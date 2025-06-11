using Godot;
using System;

public partial class AnimationController : AnimatedSprite2D
{
	[Export] private Player player;
    [Export] private float walkThreshold = 1.0f;

	private string currentState = ANIM_DEFAULT;

    const string ANIM_DEFAULT = "default";
    const string ANIM_IDLE = "idle";
    const string ANIM_WALK = "walk";
    const string ANIM_JUMPING = "jumping";
    const string ANIM_FALLING = "falling";


    // Is the player walking enough to warrent an animation change
    private bool IsWalking {  get { return Math.Abs(player.Velocity.X) > walkThreshold; } }

    public override void _Process(double delta)
    {
        SpinWhenFallingTooLong();
        
        Play();

        // TODO: Use state machine

        // TEMP
        // TODO: Should use seperate spritesheet
        if (Math.Abs(player.Velocity.X) > 0)
            FlipH = player.Velocity.X < 0;
        
		string targetState = GetTargetState();


        if (targetState == ANIM_WALK)
            SpeedScale = Math.Abs(player.Velocity.X) / 60;
        else
            SpeedScale = 1;


        if (targetState == currentState) return;
        currentState = targetState;

        Animation = currentState;
    }

    public string GetTargetState()
	{
		if (!player.IsOnFloor())
        {
            if (player.Velocity.Y < 0)
                return ANIM_JUMPING;
            
    		return ANIM_FALLING;
        }

        if (IsWalking)
            return ANIM_WALK;

		return ANIM_IDLE;
    }


    private float rotateVelocity = 0;
    private void SpinWhenFallingTooLong()
    {
        if (player.TimeSinceLastOnFloor > 1.5f)
        {
            Rotate(rotateVelocity);
            rotateVelocity += 0.001f;
            rotateVelocity = Math.Clamp(rotateVelocity, 0, 2);
        }
        else
        {
            Rotation = 0;
            rotateVelocity = 0;
        }
    }
}
