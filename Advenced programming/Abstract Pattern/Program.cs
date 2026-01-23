namespace Abstract_Pattern
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Audio Player State Pattern
            var audioPlayer = new AudioPlayer(new StoppedState());
            audioPlayer.Pause();    // from stopped: should remain stopped
            audioPlayer.Play();     // stopped -> playing
            audioPlayer.Pause();    // playing -> paused
            audioPlayer.Play();     // paused -> playing
            audioPlayer.Stop();     // playing -> stopped
            audioPlayer.Stop();     // already stopped
            audioPlayer.Pause();    // stopped
            audioPlayer.Play();     // stopped -> playing
            audioPlayer.Stop();     // playing -> stopped
            audioPlayer.Pause();    // stopped
            audioPlayer.Play();     // stopped -> playing
            audioPlayer.Play();     // already playing
            audioPlayer.Stop();     // playing -> stopped
        }
    }

    internal abstract class IState
    {
        internal AudioPlayer Player { get; set; }

        internal abstract void Play();
        internal abstract void Pause();
        internal abstract void Stop();

        internal virtual string Status => GetType().Name;
    }

    internal class AudioPlayer
    {
        private IState _state;

        public AudioPlayer(IState state)
        {
            SetState(state);
        }

        public void SetState(IState state)
        {
            _state = state;
            _state.Player = this;
            Console.WriteLine($"State changed to: {_state.Status}");
        }

        public void Play() => _state.Play();
        public void Pause() => _state.Pause();
        public void Stop() => _state.Stop();
    }

    internal class StoppedState : IState
    {
        internal override void Play()
        {
            Console.WriteLine("From Stopped to Playing");
            Player.SetState(new PlayingState());
        }

        internal override void Pause()
        {
            Console.WriteLine("Stopped State");
        }

        internal override void Stop()
        {
            Console.WriteLine("Already stopped");
        }
    }

    internal class PlayingState : IState
    {
        internal override void Play()
        {
            Console.WriteLine("Already playing");
        }

        internal override void Pause()
        {
            Console.WriteLine("From Playing to Paused");
            Player.SetState(new PausedState());
        }

        internal override void Stop()
        {
            Console.WriteLine("From Playing to Stopped");
            Player.SetState(new StoppedState());
        }
    }

    internal class PausedState : IState
    {
        internal override void Play()
        {
            Console.WriteLine("From Paused to Playing");
            Player.SetState(new PlayingState());
        }

        internal override void Pause()
        {
            Console.WriteLine("Already paused");
        }

        internal override void Stop()
        {
            Console.WriteLine("From Paused to Stopped");
            Player.SetState(new StoppedState());
        }
    }
}
