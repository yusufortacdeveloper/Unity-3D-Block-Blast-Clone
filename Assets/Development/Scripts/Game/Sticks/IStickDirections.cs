public enum StickDirections
{
    Horizontal,
    Vertical
}
public interface IStickDirections
{
    StickDirections GetStickDirection();
}
