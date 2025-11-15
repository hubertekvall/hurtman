namespace Hurtman.Actor;

public partial class DeathMessage  (DeathCause cause): ActorMessage
{
    private DeathCause Cause { get; set; } = cause;
}