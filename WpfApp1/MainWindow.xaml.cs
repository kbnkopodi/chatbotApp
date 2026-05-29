using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Speech.Synthesis;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
   
    public partial class MainWindow : Window
    {
        private SpeechSynthesizer voice;
        private Dictionary<string, string> responses;
        private List<string> chatMemory;
        private string memoryFile = "chatmemory.txt";
        private string profileFile = "userprofile.txt";
        private Dictionary<string, string> emotions;
        private string lastTopic = "";
        private string lastBotResponse = "";
        private Dictionary<string, string> followUps;
        private string userName = "";
        private string favoriteTopic = "";

        public MainWindow()
        {
            InitializeComponent();          
            voice = new SpeechSynthesizer();
            LoadUserProfile();
            if (string.IsNullOrEmpty(userName))
            {
                AddBotMessage("Hello! What is your name?");
            }
            else
            {
                AddBotMessage("Welcome back " + userName + "!");
                if (!string.IsNullOrEmpty(favoriteTopic))
                {

                    AddBotMessage("I remember your favorite cybersecurity topic is " + favoriteTopic + ".");
                }
            }


            StartIntroduction();
            chatMemory = new List<string>();
            responses = new Dictionary<string, string>()
            {
                {"how are you", "I'm doing great! Always ready to help you stay safe online."},

                {"whats your purpose",
                "My purpose is to help you stay safe online! I can teach you about phishing, password safety and safe browsing."},

                {"what can i ask you about",
                "You can ask me about phishing, password safety and safe browsing."},

                {"phishing",
                "Phishing is an online scam where attackers trick people into giving sensitive information."},

                {"safe browsing",
                "Safe browsing means using the internet in a secure way to protect your information and device."},

                {"password safety",
                "Password safety means creating strong passwords and protecting them from hackers."},

                   {"what do you remember",
                GetMemorySummary()}
            };
            LoadMemory();

            emotions = new Dictionary<string, string>()
            {
                {"worried", "I understand this can feel stressful. Cybersecurity can seem overwhelming at first, but you're learning step by step."},

                {"scared", "There's no need to panic. Staying informed and cautious already makes you much safer online."},

                {"confused", "That's okay. Cybersecurity concepts can be confusing at first. I can explain things more simply if you'd like."},

                {"frustrated", "I understand your frustration. Let's break the topic down into smaller and easier parts."},

                {"curious", "Curiosity is great in cybersecurity. Learning how threats work helps you stay protected online."},

                {"overwhelmed", "Take it one step at a time. Cybersecurity is a huge field, and nobody learns everything immediately."},

                {"unsure", "It's completely normal to feel unsure. Asking questions is the best way to improve your cybersecurity knowledge."}
            };

            followUps = new Dictionary<string, string>()
{
            {"phishing",
                "One major phishing warning sign is urgent language like 'your account will be suspended immediately.' Always verify emails before clicking links."},

            {"password safety",
                "Another good security habit is enabling two-factor authentication on important accounts."},

            {"safe browsing",
                "You should also check whether websites use HTTPS before entering personal information."},

            {"malware",
                "Malware can spread through unsafe downloads, fake apps, and malicious email attachments."},

            {"vpn",
                "VPNs are especially useful on public Wi-Fi networks because they help protect your data from attackers."}
            };

            AddBotMessage("Hello! Welcome to CyberSecurity Awareness Bot.");
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string input = UserInput.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(input))
                return;
            // SAVE USER NAME 

            if (string.IsNullOrEmpty(userName))
            {
                AddUserMessage(input);
                userName = input;
                SaveUserProfile();
                string greeting =
                    "Nice to meet you " + userName +
                    "! What is your favorite cybersecurity topic?";
                AddBotMessage(greeting);
                voice.SpeakAsync(greeting);
                UserInput.Clear();
                return;
            }

            // SAVE FAVORITE TOPIC 

            if (string.IsNullOrEmpty(favoriteTopic))
            {
                favoriteTopic = input;
                SaveUserProfile();
                string topicReply =
                    "Awesome! I'll remember that you're interested in " +
                    favoriteTopic + ".";
                AddBotMessage(topicReply);
                voice.SpeakAsync(topicReply);
                UserInput.Clear();
                return;
            }
            // ADD USER MESSAGE
            AddUserMessage(input);
            // SAVE MEMORY
            SaveMessage("USER: " + input);
            // EXIT
            if (input == "bye")
            {
                string goodbye = "Goodbye " +"! Stay safe online!";
                AddBotMessage(goodbye);
                SaveMessage("BOT: " + goodbye);
                SoundPlayer player = new SoundPlayer("Bye.wav");
                player.Play();
                UserInput.Clear();
                return;
            }

            // MEMORY QUESTIONS
            if (input.Contains("remember"))
            {
                string memoryReply = GetMemorySummary();
                AddBotMessage(memoryReply);
                voice.SpeakAsync(memoryReply);
                SaveMessage("BOT: " + memoryReply);
                UserInput.Clear();
                return;
            }
            // SENTIMENT DETECTION 

            foreach (var emotion in emotions)
            {
                if (input.Contains(emotion.Key))
                {
                    AddBotMessage(emotion.Value);
                    voice.SpeakAsync(emotion.Value);
                    SaveMessage("BOT: " + emotion.Value);
                    UserInput.Clear();
                    return;
                }
            }
            //  FOLLOW-UP DETECTION 

            if (input.Contains("tell me more") ||
                input.Contains("explain more") ||
                input.Contains("another tip") ||
                input.Contains("what else") ||
                input.Contains("why") ||
                input.Contains("how"))
            {
                if (!string.IsNullOrEmpty(lastTopic) &&
                    followUps.ContainsKey(lastTopic))
                {
                    string followReply = followUps[lastTopic];
                    AddBotMessage(followReply);
                    voice.SpeakAsync(followReply);
                    SaveMessage("BOT: " + followReply);
                    UserInput.Clear();
                    return;
                }
            }

            // NORMAL RESPONSES
            if (responses.TryGetValue(input, out string reply))
            {
                lastTopic = input;
                lastBotResponse = reply;
                AddBotMessage(reply);
                voice.SpeakAsync(reply);
                SaveMessage("BOT: " + reply);
            }
            else
            {
                string unknown =
                    "I don't fully understand that yet, but I'm learning from our conversations.";
                AddBotMessage(unknown);
                SaveMessage("BOT: " + unknown);
            }
            UserInput.Clear();
        }
       

        // SAVE MEMORY 
        private void SaveMessage(string message)
        {
            chatMemory.Add(message);
            File.AppendAllText(memoryFile, message + Environment.NewLine);
        }

        //  LOAD MEMORY 
        private void LoadMemory()
        {
            if (File.Exists(memoryFile))
            {
                var lines = File.ReadAllLines(memoryFile);
                foreach (var line in lines)
                {
                    chatMemory.Add(line);
                    // DISPLAY OLD CHAT
                    if (line.StartsWith("USER:"))
                    {
                        AddUserMessage(line.Replace("USER: ", ""));
                    }
                    else if (line.StartsWith("BOT:"))
                    {
                        AddBotMessage(line.Replace("BOT: ", ""));
                    }
                }
            }
        }

        // MEMORY SUMMARY 
        private string GetMemorySummary()
        {
            if (chatMemory.Count == 0)
            {
                return "I currently have no saved memories.";
            }
            // GET LAST 5 MESSAGES
            var recentMessages = chatMemory.TakeLast(5);
            return "Here are some things I remember: "
                   + string.Join(" | ", recentMessages);
        }

        private void AddUserMessage(string message)
        {
            StackPanel messageContainer = new StackPanel
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(200, 10, 0, 10)
            };

            Border bubble = new Border
            {
                Background = Brushes.LimeGreen,
                CornerRadius = new CornerRadius(15),
                Padding = new Thickness(15),
                MaxWidth = 400
            };

            TextBlock text = new TextBlock
            {
                Text = message,
                Foreground = Brushes.Black,
                FontSize = 15,
                TextWrapping = TextWrapping.Wrap
            };

            bubble.Child = text;

            // TIMESTAMP
            TextBlock timeStamp = new TextBlock
            {
                Text = DateTime.Now.ToString("HH:mm"),
                Foreground = Brushes.LightGray,
                FontSize = 11,
                Margin = new Thickness(5, 2, 5, 0),
                HorizontalAlignment = HorizontalAlignment.Right
            };
            messageContainer.Children.Add(bubble);
            messageContainer.Children.Add(timeStamp);
            ChatPanel.Children.Add(messageContainer);
            ChatScrollViewer.ScrollToEnd();
        }
        private void AddBotMessage(string message)
        {
            StackPanel messageContainer = new StackPanel
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(0, 10, 200, 10)
            };

            Border bubble = new Border
            {
                Background = Brushes.DimGray,
                CornerRadius = new CornerRadius(15),
                Padding = new Thickness(15),
                MaxWidth = 400
            };

            TextBlock text = new TextBlock
            {
                Text = message,
                Foreground = Brushes.White,
                FontSize = 15,
                TextWrapping = TextWrapping.Wrap
            };

            bubble.Child = text;

            // TIMESTAMP
            TextBlock timeStamp = new TextBlock
            {
                Text = DateTime.Now.ToString("HH:mm"),
                Foreground = Brushes.Gray,
                FontSize = 11,
                Margin = new Thickness(5, 2, 5, 0)
            };

            messageContainer.Children.Add(bubble);
            messageContainer.Children.Add(timeStamp);
            ChatPanel.Children.Add(messageContainer);
            ChatScrollViewer.ScrollToEnd();
        }

        private void NewChatButton_Click(object sender, RoutedEventArgs e)
        {
            ChatPanel.Children.Clear();
            chatMemory.Clear();
            File.WriteAllText(memoryFile, "");
            lastTopic = "";
            AddBotMessage("New secure chat started. How can I help you today?");
        }

        private void StartIntroduction()
        {
            string intro =
                "Welcome to Cyber Security AI Assistant. " +
                "Systems are now online and secure.";
            AddBotMessage(intro);
            voice.SpeakAsync(intro);
        }

        private void UserInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendButton_Click(sender, e);
            }
        }

        private void SaveUserProfile()
        {
            File.WriteAllLines(profileFile, new string[]
            {
        userName,
        favoriteTopic
            });
        }

        private void LoadUserProfile()
        {
            if (File.Exists(profileFile))
            {
                var lines = File.ReadAllLines(profileFile);
                if (lines.Length > 0)
                    userName = lines[0];
                if (lines.Length > 1)
                    favoriteTopic = lines[1];
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            // CLEAR CHAT UI
            ChatPanel.Children.Clear();
            // CLEAR MEMORY
            chatMemory.Clear();
            // RESET USER INFO
            userName = "";
            favoriteTopic = "";
            // RESET CONTEXT
            lastTopic = "";
            lastBotResponse = "";
            // DELETE FILES
            if (File.Exists(memoryFile))
            {
                File.Delete(memoryFile);
            }

            if (File.Exists(profileFile))
            {
                File.Delete(profileFile);
            }

            // RESTART CHATBOT
            AddBotMessage("Memory reset complete.");
            AddBotMessage("Hello! What is your name?");
            voice.SpeakAsync("Memory reset complete. What is your name?");
        }


    }
}