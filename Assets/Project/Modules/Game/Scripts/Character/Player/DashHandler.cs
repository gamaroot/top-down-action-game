using UnityEngine;

public class DashHandler
{
    public bool CanDash { get; private set; } = true;
    public bool IsDashing { get; private set; }
    public Vector3 DashMovement { get; private set; }

    private readonly float _dashSpeed;
    private readonly float _dashDuration;
    private readonly float _dashCooldown;

    private float _dashStartTime;     // Track the start time of the dash
    private Vector3 _dashDirection;   // Dash direction
    private float _dashCooldownTimer; // Timer for dash cooldown

    public DashHandler(float dashSpeed, float dashDuration, float dashCooldown)
    {
        this._dashSpeed = dashSpeed;
        this._dashDuration = dashDuration;
        this._dashCooldown = dashCooldown;
    }

    public void Dash(Vector2 dashInput)
    {
        if (!this.CanDash && dashInput == Vector2.zero)
            return;

        this.CanDash = false;
        this.IsDashing = true;

        this._dashStartTime = Time.time;  // Set the start time of the dash
        this._dashDirection = new Vector3(dashInput.x, 0, dashInput.y).normalized;

        // Start the cooldown timer
        this._dashCooldownTimer = this._dashCooldown;
    }

    public void OnUpdate()
    {

        if (this.IsDashing)
        {
            float dashElapsedTime = Time.time - this._dashStartTime;
            if (dashElapsedTime < this._dashDuration)
            {
                // Continue dashing in the stored direction
                this.DashMovement = this._dashDirection * this._dashSpeed * Time.deltaTime;
            }
            else
            {
                // Dash ended
                this.IsDashing = false;
            }
            return;
        }

        // Handle dash cooldown
        if (this._dashCooldownTimer <= 0f)
        {
            this.CanDash = true;
            this._dashCooldownTimer = 0;
        }
        else
        {
            this._dashCooldownTimer -= Time.deltaTime;
        }
    }
}
