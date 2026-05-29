🛡️ CyberSecurity AI Assistant (WPF Chatbot)

An interactive AI-style cybersecurity awareness chatbot built using C# WPF (.NET).
The application simulates a smart assistant that teaches users about online safety, phishing, password security, safe browsing, and general cybersecurity awareness.

It includes voice responses, memory persistence, emotion detection, and a modern cyber-themed UI.

✨ Features
🤖 Chatbot Intelligence
Keyword-based response system
Cybersecurity topic explanations:
Phishing
Password safety
Safe browsing
Malware awareness
VPN usage
Follow-up suggestions for deeper learning


🧠 Memory System
Saves chat history locally (chatmemory.txt)
Remembers user name and favorite topic (userprofile.txt)
Displays recent conversation history
Persistent memory across sessions


😊 Emotion Detection

The bot can detect user sentiment and respond appropriately:

Worried
Scared
Confused
Frustrated
Curious
Overwhelmed
Unsure


🔊 Voice Assistant
Uses SpeechSynthesizer to speak responses
Creates a more interactive AI experience


💬 Chat Interface (WPF UI)
Modern dark cyber-themed UI
Chat bubbles for user and bot messages
Timestamped messages
Scrollable chat panel
Sidebar system dashboard

🔐 System Features
New Chat reset function
Full memory reset option
Safe file-based storage system
Session persistence
🖥️ UI Preview

Cyber-themed interface with:

Left sidebar dashboard (system status)
Main chat window
Input box + send button
Cybersecurity branding panel
📁 Project Structure
WpfApp1/
│
├── MainWindow.xaml          # UI Design (WPF layout)
├── MainWindow.xaml.cs      # Chatbot logic (C# backend)
│
├── chatmemory.txt          # Stores conversation history
├── userprofile.txt         # Stores user name + favorite topic
├── Bye.wav                 # Exit sound effect
⚙️ Technologies Used
C# (.NET WPF)
Windows Presentation Foundation (WPF)
System.Speech (Voice synthesis)
File I/O (Memory persistence)
XAML (UI design)


🚀 How to Run
Clone the repository:
git clone https://github.com/yourusername/cybersecurity-ai-assistant.git
Open the project in Visual Studio
Restore NuGet packages (if needed)
Run the application (Start Debugging or F5)



💡 Future Improvements
🔥 AI integration (OpenAI / ChatGPT API)
📊 Cybersecurity quiz system
🌐 Real-time threat news feed
🧠 Natural language processing (better understanding)
🗄️ Database storage instead of text files
🎤 Voice input (speech recognition)
⌨️ Typing animation like ChatGPT


🎯 Purpose of the Project

This project was created to:

Teach basic cybersecurity awareness
Demonstrate WPF UI development skills
Show chatbot logic using C#
Simulate AI-style interaction without external APIs


👨‍💻 Author

Kabelo Nkopodi
Cybersecurity AI Assistant Developer
🇿🇦 South Africa


📜 License

This project is for educational purposes.
