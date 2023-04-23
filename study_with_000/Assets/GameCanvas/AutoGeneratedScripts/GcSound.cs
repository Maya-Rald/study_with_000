/*------------------------------------------------------------*/
// <summary>GameCanvas for Unity</summary>
// <author>Seibe TAKAHASHI</author>
// <remarks>
// (c) 2015-2020 Smart Device Programming.
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
// </remarks>
/*------------------------------------------------------------*/
namespace GameCanvas
{
    public readonly partial struct GcSound : System.IEquatable<GcSound>
    {
        internal const int __Length__ = 8;
        public static readonly GcSound Correct = new GcSound("GcSoundCorrect");
        public static readonly GcSound DoorLocked = new GcSound("GcSoundDoorLocked");
        public static readonly GcSound DoorOpen = new GcSound("GcSoundDoorOpen");
        public static readonly GcSound HorrorBgm = new GcSound("GcSoundHorrorBgm");
        public static readonly GcSound HorrorPiano = new GcSound("GcSoundHorrorPiano");
        public static readonly GcSound Incorrect_short = new GcSound("GcSoundIncorrect_short");
        public static readonly GcSound Mumble = new GcSound("GcSoundMumble");
        public static readonly GcSound WhiteNoise = new GcSound("GcSoundWhiteNoise");
    }
}
