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
    public class ChooseFilmForm
    {
        public static List<string> moviesList = new List<string>() ;
        public static string username { get; set; }

        [Prompt("{||}")]
        public string MovieName { get; set; }

        public static IForm<ChooseFilmForm> BuildMoviesForm()
        {

            return new FormBuilder<ChooseFilmForm>()
                    .Message(username+ ", вот фильмы идущие на этой неделе:")
                    .Field(new FieldReflector<ChooseFilmForm>(nameof(MovieName))
                            .SetType(null)
                            .SetDefine((state, field) =>
                            {
                                foreach (var prod in GetMovies())
                                    field
                                        .AddDescription(prod, prod)
                                        .AddTerms(prod, prod);

                                return Task.FromResult(true);
                            }))
                    .AddRemainingFields()
                    .Build();
        }

        public static IFormDialog<ChooseFilmForm> BuildMoviesDialog(FormOptions options = FormOptions.PromptInStart)
        {
            // Generated a new FormDialog<T> based on IForm<BasicForm>
            return FormDialog.FromForm(BuildMoviesForm, options);
        }

        static List<string> GetMovies()
        {
            return moviesList;
        }
    }
}