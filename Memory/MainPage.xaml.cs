using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Memory;

public partial class MainPage : ContentPage, INotifyPropertyChanged
{
    const int GridSize = 4; // 4x4 = 16 carte
    private int tries;
    public int Tries
    {
        get => tries;
        set
        {
            tries = value;
            OnPropertyChanged(nameof(Tries));
        }
    }
    private int score;
    public int Score
    {
        get => score;
        set
        {
            score = value;
            OnPropertyChanged(nameof(Score));
        }
    }

    private readonly string[] requiredImages =
    [
        "card_10.png", "card_11.png", "card_20.png", "card_21.png",
        "card_30.png", "card_31.png", "card_40.png", "card_41.png",
        "card_50.png", "card_51.png", "card_60.png", "card_61.png",
        "card_70.png", "card_71.png", "card_80.png", "card_81.png"
    ];
    private List<Frame> deck = [];
    private List<Frame> extractedCards = [];
    private List<Frame> selectedCard = [];
    private List<Frame> matchedCards = [];

    public MainPage()
    {
        InitializeComponent();
        BindingContext = this;
        GenerateCardGrid();
    }

    private void GenerateCardGrid()
    {
        // Definizione righe e colonne
        for (int i = 0; i < GridSize; i++)
        {
            CardGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
            CardGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
        }

        for (int i = 0; i < requiredImages.Length; i++)
        {
            var card = CreateCard(requiredImages[i]);
            if (card != null)
                deck.Add(card);
        }

        // Creazione delle carte
        for (int row = 0; row < GridSize; row++)
        {
            for (int col = 0; col < GridSize; col++)
            {
                var card = ExtractCard();
                // Imposto posizione nella griglia
                Grid.SetRow(card, row);
                Grid.SetColumn(card, col);

                // Aggiungo alla griglia
                CardGrid.Children.Add(card);
            }
        }
    }


    // EXTRACT A RANDOM CARD FROM THE DECK
    private Frame? ExtractCard()
    {
        // RETURN NULL IF THE DECK IS EMPTY
        if (deck.Count == 0)
            return null;

        var random = new Random();
        int index = random.Next(deck.Count);
        var card = deck[index];
        deck.RemoveAt(index);
        extractedCards.Add(card);
        return card;
    }

    private Frame CreateCard(string cardValue)
    {
        if (cardValue.Contains("card_back"))
            return null;

        var image = new Image
        {
            Source = "card_back.png", // retro iniziale
            Aspect = Aspect.AspectFit
        };

        var frame = new Frame
        {
            CornerRadius = 8,
            BackgroundColor = Color.FromArgb("#4B3FA6"),
            Padding = 0,
            Margin = 5,
            HasShadow = true,
            Content = image,
        };

        // Evento di click
        var tapGesture = new TapGestureRecognizer();
        tapGesture.Tapped += (s, e) =>
        {
            // RETURN IF TWO CARDS ARE ALREADY SELECTED
            if (matchedCards.Contains(frame) || selectedCard.Count == 2 || selectedCard.Contains(frame))
                return;

            // ADD TRY IF THE CARD IS SHOWING THE BACK OF THE CARD
            if (!image.Source.Equals(cardValue))
            {
                Tries++;
            }
            // SHOW THE CARD VALUE
            image.Source = cardValue;
            // ADD THE CARD TO THE SELECTED CARDS
            selectedCard.Add(frame);
            // CHECK IF TWO CARDS ARE SELECTED
            if (selectedCard.Count == 2)
            {
                CheckMatchingCards();
            }
        };
        // ADD THE GESTURE RECOGNIZER TO THE FRAME
        frame.GestureRecognizers.Add(tapGesture);

        return frame;
    }

    // GET THE VALUES OF THE TWO SELECTED CARDS
    private int GetCardName(Frame card)
    {
        int value = -1;

        // EXTRACT AND RETURN THE CARD NUMBER FROM THE IMAGE FILENAME
        if (card.Content is Image image && image.Source is FileImageSource fileImageSource)
        {
            var cardName = Path.GetFileNameWithoutExtension(fileImageSource.File);
            return int.Parse(cardName.Split('_')[1]);

        }
        return value;
    }

    // CHECK IF THE TWO SELECTED CARDS MATCH
    private void CheckMatchingCards()
    {
        var firstCardValue = GetCardName(selectedCard[0]);
        var secondCardValue = GetCardName(selectedCard[1]);

        // CHECK IF THE CARDS ARE FROM THE SAME SET
        if (firstCardValue == secondCardValue + 1 || firstCardValue == secondCardValue - 1)
        {
            // IF THE CARDS MATCH, ADD THEM TO THE MATCHED CARDS LIST
            matchedCards.AddRange(selectedCard);
            // CLEAR THE SELECTED CARDS LIST
            selectedCard.Clear();
            // INCREASE THE SCORE
            Score += 10;
            // CHECK IF ALL CARDS ARE MATCHED
            if (matchedCards.Count == extractedCards.Count && extractedCards.Count > 0)
            {
                WinnerAlert();
            }
        }
        else
        {
            // IF THE CARDS DO NOT MATCH, FLIP THEM BACK AFTER A SHORT DELAY AND CLEAR THE SELECTED CARDS LIST
            Task.Delay(750).ContinueWith(_ =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    foreach (var card in selectedCard)
                    {
                        if (card.Content is Image image)
                        {
                            image.Source = "card_back.png";
                        }
                    }
                    selectedCard.Clear();
                });
            });
        }
    }

    // RESET BUTTON CLICK EVENT
    private void OnResetButtonClicked(object sender, EventArgs e)
    {
        CardGrid.Children.Clear();
        CardGrid.RowDefinitions.Clear();
        CardGrid.ColumnDefinitions.Clear();
        deck.Clear();
        extractedCards.Clear();
        selectedCard.Clear();
        Tries = 0;
        Score = 0;
        GenerateCardGrid();
    }

    // SHOW ALERT WHEN THE PLAYER WINS
    private void WinnerAlert()
    {
        DisplayAlert("Congratulations!", $"You completed the game in {Tries} tries and with a score of {Score} points!\nYour final score is {score - Tries}!", "OK");
    }

    // INotifyPropertyChanged IMPLEMENTATION FOR DATA BINDING
    public new event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
