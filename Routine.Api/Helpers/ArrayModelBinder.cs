using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Routine.Api.Helpers
{
    public class ArrayModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (!bindingContext.ModelMetadata.IsEnumerableType)
            {
                bindingContext.Result = ModelBindingResult.Failed();
                return Task.CompletedTask;
            }
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).ToString();
            if (string.IsNullOrWhiteSpace(value))
            {
                bindingContext.Result = ModelBindingResult.Success(null);
            }
            var elementType = bindingContext.ModelType.GetTypeInfo().GenericTypeArguments[0];
            var convertor = TypeDescriptor.GetConverter(elementType); //返回指定类型的类型转换器
            //用类型转换器把参数转换为指定类型
            var values = value.Split(new[] { ","},StringSplitOptions.RemoveEmptyEntries).Select(x=>convertor.ConvertFromString(x.Trim())).ToArray();

            var typedValues = Array.CreateInstance(elementType,values.Length); //创建一个指定类型的Array数组
            values.CopyTo(typedValues,0);
            bindingContext.Model = typedValues;

            bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);
            return Task.CompletedTask;
        }
    }
}
