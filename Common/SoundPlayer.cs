namespace OptimaSync.Common
{
    class SoundPlayer
    {
        public static void PlayNotificationSound()
        {
            var player = new System.Media.SoundPlayer();
            player.Stream = Properties.Resources.notification_sound;
            player.Play();
        }
    }
}
