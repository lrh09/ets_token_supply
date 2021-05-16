using System.Reflection;
using Omu.ValueInjecter.Injections;

namespace WebApi.Services
{
    public class IgnoreNullInjection: LoopInjection
    {
        protected override void SetValue(object source, object target, PropertyInfo sp, PropertyInfo tp)
        {
            if (sp.GetValue(source) == null) return;
            base.SetValue(source, target, sp, tp);
        }
    }
}