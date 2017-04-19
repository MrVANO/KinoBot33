using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.FormFlow.Advanced;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace KinoBot2.Forms
{
    [Serializable]
    public class ChooseDateForm
    {
        public static List<string> datesList = new List<string>();

        [Prompt("Даты показа {||}")]
        public string Date { get; set; }

        public static IForm<ChooseDateForm> BuildDatesForm()
        {

            return new FormBuilder<ChooseDateForm>()
                    .Field(new FieldReflector<ChooseDateForm>(nameof(Date))
                            .SetType(null)
                            .SetDefine((state, field) =>
                            {
                                foreach (var prod in GetDates())
                                    field
                                        .AddDescription(prod, prod)
                                        .AddTerms(prod, prod);

                                return Task.FromResult(true);
                            }))
                    .AddRemainingFields()
                    .Build();
        }

        public static IFormDialog<ChooseDateForm> BuildDatesDialog(FormOptions options = FormOptions.PromptInStart)
        {
            // Generated a new FormDialog<T> based on IForm<BasicForm>
            return FormDialog.FromForm(BuildDatesForm, options);
        }

        static List<string> GetDates()
        {
            return datesList;
        }
    }
}