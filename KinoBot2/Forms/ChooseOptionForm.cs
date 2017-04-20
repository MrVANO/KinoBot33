using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KinoBot2.Forms
{
    public enum Options {IGNORE, ФильмыНаНеделе};

    [Serializable]
    public class ChooseOptionForm
    {
        public static string message="";
        [Template(TemplateUsage.EnumSelectOne, "Выберите действие {||}")]
        public Options actionOptions { get; set; }

        public static IForm<ChooseOptionForm> BuildOptionsForm()
        {
            IFormBuilder<ChooseOptionForm> builder = new FormBuilder<ChooseOptionForm>()
                    //.Message(message)
                    .Field(nameof(actionOptions)).AddRemainingFields();

            return builder.Build();
        }

        public static IFormDialog<ChooseOptionForm> BuildOptionsDialog(FormOptions options = FormOptions.PromptInStart)
        {
            // Generated a new FormDialog<T> based on IForm<BasicForm>
            return FormDialog.FromForm(BuildOptionsForm, options);
        }
    }
}