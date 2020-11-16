using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Casino
{

    public partial class Form1 : Form
    {
        DeckOfCards Deck = new DeckOfCards();

        public class random
        {
            public static Random randomizer = new Random();

            public int get1Roll()
            {
                int ret = randomizer.Next(6);
                return ret;
            }

            public int get2Roll()
            {
                int ret = randomizer.Next(1, 7);
                return ret;
            }

            public static int getFlip()
            {
                int ret = randomizer.Next(0, 2);
                return ret;
            }

            public int curCard(int n)
            {
                int ret = randomizer.Next(n);
                return ret;
            }

            public static int shuffler(int n)
            {
                int ret = randomizer.Next(0, n);
                return ret;
            }
        }
        public class Die : random
        {
            public int currentRoll; // Variable holds current roll value

            public int rollDie(int n)
            { // function "rolls" the die object
                if(n == 1)
                {
                    currentRoll = get1Roll();
                }
                else
                {
                    currentRoll = get2Roll();
                }
                return currentRoll;
            }

            public static implicit operator int(Die D)
            { // implicitly returns the current roll as an integer
                return D.currentRoll;
            }

            public static int operator +(Die left, Die right) // adds two die object roll results together
            {
                return left.currentRoll + right.currentRoll;
            }
        }

        public class Coin : random
        {
            public static int curVal;
            //public static Random flipper = new Random();
            public static List<int> coinCount = new List<int>();

            public static implicit operator string(Coin C) // implicitly returns a coin flip result as a string
            {
                curVal = getFlip();
                if (curVal == 0) return "HEADS";
                if (curVal == 1) return "TAILS";
                else return "error";
            }
        }

        public class Card
        {
            public int value;
            public int suit;

            public Card() { }

            public Card(int v, int s)
            {
                value = v;
                suit = s;
            }

            // If inmplicitly called as a string return the card's value and suite
            public static implicit operator string(Card C)
            {
                string cardname = "";
                switch (C.value)
                {
                    case 11:
                        cardname = "Jack of ";
                        break;
                    case 12:
                        cardname = "Queen of ";
                        break;
                    case 13:
                        cardname = "King of ";
                        break;
                    case 14:
                        cardname = "Ace of ";
                        break;
                    default:
                        cardname = C.value.ToString() + " of ";
                        break;
                }
                switch (C.suit)
                {
                    case 0:
                        cardname = cardname + "Hearts";
                        break;
                    case 1:
                        cardname = cardname + "Diamonds";
                        break;
                    case 2:
                        cardname = cardname + "Clubs";
                        break;
                    case 3:
                        cardname = cardname + "Spades";
                        break;
                }
                return cardname;
            }
        }

        public class DeckOfCards : random
        {
            //public List<Card> Cards { get; set; }
            public int numCards;
            //public List<Card> Deck = new List<Card>();
            public List<Card> Cards = new List<Card>();

            public DeckOfCards()
            {
                numCards = 0;
                //Cards = new List<Card>();

                //Populate Deck by first generating suits...
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 2; j < 15; j++)
                    {
                        Card tempCard = new Card(j, i);
                        Cards.Insert(numCards, tempCard);
                        numCards++;
                    }
                }
            }

            public void Shuffle() //Method for shuffling a deck, called with every implicit call of the deck
            {
                //Random rand = new Random();
                Card temp = new Card();

                for (int i = 0; i < 100000; i++)
                {
                    int i1 = curCard(numCards);
                    int i2 = curCard(numCards);

                    temp = Cards[i1];
                    Cards[i1] = Cards[i2];
                    Cards[i2] = temp;
                }
            }

            public static implicit operator Card(DeckOfCards D) // Implicitly returns a card object at random from the deck
            {
                Card C = new Card();

                D.Shuffle(); // runs the above shuffle method

                if (D.numCards > 0)
                {
                    //Random r = new Random();
                    C = D.Cards[shuffler(D.Cards.Count)];
                    D.Cards.Remove(C);
                    D.numCards--;
                }

                return C;
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        // button1_Click handles Button Click action that runs the program
        private void button1_Click(object sender, EventArgs e)
        {
            // Generate Lists to hold counts for rolled values
            List<int> count1 = new List<int>();
            List<int> count2 = new List<int>();
            List<int> count3 = new List<int>();

            // Bind the charts to their respective arrays of counts
            chart1.Series[0].Points.DataBindY(count1);
            chart2.Series[0].Points.DataBindY(count2);
            chart3.Series[0].Points.DataBindY(count3);

            // initialize die 1 count
            for (int i = 1; i <= 6; i++) { count1.Add(0); }
            for (int i = 1; i <= 6; i++) { count3.Add(0); }

            // initialize double die count
            for (int i = 0; i <= 11; i++) { count2.Add(0); }

            // Make Random functions for two seperate "dice"
            //Random randomizer = new Random();
            Die die1 = new Die();
            Die die2 = new Die();
            Die die3 = new Die();

            // Pull and parse to integer the values in the textbox
            int nRolls = int.Parse(numRolls.Text);

            // Compute die rolls n times
            for (int i = 0; i < nRolls; i++)
            {
                // Computes the results for single die rolls on first dice
                die1.rollDie(1);
                int currRoll = die1;
                count1[currRoll] += 1;
                // Updates chart 1
                chart1.Series[0].Points.DataBindY(count1);
                chart1.Update();

                // Computes the results for single die rolls on 2nd dice
                die3.rollDie(1);
                int currRoll3 = die3;
                count3[currRoll3] += 1;
                //Updates chart 3 
                chart3.Series[0].Points.DataBindY(count3);
                chart3.Update();

                // Computes the results for the sum of two die rolls
                die2.rollDie(2);
                die2.rollDie(2);
                int currRoll2 = die1 + die2 - 1;
                count2[currRoll2] += 1;
                // Updates chart 2
                chart2.Series[0].Points.DataBindY(count2);
                chart2.Update();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Coin coin1 = new Coin();
            string result = coin1;
            coinTextResult.Text = result;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            List<int> coinCount = new List<int>();
            chart5.Series[0].Points.DataBindY(coinCount);
            for (int i = 0; i <= 1; i++) { coinCount.Add(0); }

            // Pull and parse to integer the values in the textbox where nRolls will be total flips
            int nRolls = int.Parse(numRolls.Text);
            Coin coin1 = new Coin();

            for (int i = 0; i < nRolls; i++)
            {
                string result = coin1;
                if (result == "HEADS") coinCount[0] += 1;
                else if (result == "TAILS") coinCount[1] += 1;

                chart5.Series[0].Points.DataBindY(coinCount);
                chart5.Update();
            }
        }

        // This method draws a card from the deck instantiated by button 5
        private void button3_Click(object sender, EventArgs e)
        {
            Card cDraw = new Card();

            if (Deck.numCards > 0)
            {
                cDraw = Deck;
                textBox3.Text = Deck.numCards.ToString();
                textBox2.Text = cDraw;
            }
            else
            {
                textBox3.Text = Deck.numCards.ToString();
                textBox2.Text = "";
            }
        }

        // This button method will reset the Deck to its original full 52 cards
        private void button5_Click(object sender, EventArgs e)
        {
            Deck = new DeckOfCards();
            textBox3.Text = Deck.numCards.ToString();
            textBox2.Text = "";
        }

        // This method draws a number of cards at once from a fresh deck
        private void button6_Click(object sender, EventArgs e)
        {
            DeckOfCards Deck5 = new DeckOfCards();
            Card cDraw = new Card();

            // add a shuffle functionality

            cDraw = Deck5;
            string retText = cDraw;
            string newLine = Environment.NewLine;

            for (int i = 0; i < 4; i++)
            {
                cDraw = Deck5;
                retText += newLine + cDraw;
            }
            textBox1.Text = retText;
        }
    }
}