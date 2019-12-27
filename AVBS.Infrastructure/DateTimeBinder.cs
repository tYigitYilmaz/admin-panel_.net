using System;
using System.Globalization;
using System.Web.Mvc;

namespace AVBS.Infrastructure {
    public class DateTimeBinder : IModelBinder {
        public object BindModel ( ControllerContext controllerContext, ModelBindingContext bindingContext ) {
            var value = bindingContext.ValueProvider.GetValue( bindingContext.ModelName );
            bindingContext.ModelState.SetModelValue( bindingContext.ModelName, value );

            return value.ConvertTo( typeof( DateTime ), CultureInfo.CurrentCulture );
        }
    }

    public class NullableDateTimeBinder : IModelBinder {
        public object BindModel ( ControllerContext controllerContext, ModelBindingContext bindingContext ) {
            var value = bindingContext.ValueProvider.GetValue( bindingContext.ModelName );
            bindingContext.ModelState.SetModelValue( bindingContext.ModelName, value );

            return value == null
                ? null
                : value.ConvertTo( typeof( DateTime ), CultureInfo.CurrentCulture );
        }
    }
}