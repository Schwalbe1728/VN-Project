public enum NodeType
{
    Exit,
    Node,
    Option,
    Condition
}

public static class NodeTypeExtension
{
    public static string ToString(this NodeType type, bool dump)
    {
        switch (type)
        {
            case NodeType.Condition:
                return "Condition";

            case NodeType.Exit:
                return "Exit";

            case NodeType.Node:
                return "Node";

            case NodeType.Option:
                return "Option";

            default:
                return null;
        }
    }
}

public enum ConditionTypes
{
    AttributeCheck,
    AttributeTest,
    SkillPossessed,
    PlayerHasItem,
    StoryStateHappened
}

public enum InequalityTypes
{
    Less,
    LessOrEqual,
    Equal,
    GreaterOrEqual,
    Greater
}

public static class InequalityTypeExtension
{
    public static string Name(this InequalityTypes type)
    {
        switch(type)
        {
            case InequalityTypes.Equal: return "Equals";
            case InequalityTypes.Greater: return "Greater Than";
            case InequalityTypes.GreaterOrEqual: return "Greater Than Or Equal To";
            case InequalityTypes.Less: return "Less Than";
            case InequalityTypes.LessOrEqual: return "Less Than Or Equal To";
            default: return "";           
        }
    }

    public static bool Value(this InequalityTypes type, int Prev, int Next)
    {
        switch(type)
        {
            case InequalityTypes.Equal:
                return Prev == Next;

            case InequalityTypes.Greater:
                return Prev > Next;

            case InequalityTypes.GreaterOrEqual:
                return Prev >= Next;

            case InequalityTypes.Less:
                return Prev < Next;

            case InequalityTypes.LessOrEqual:
                return Prev <= Next;

            default: return false;
        }
    }
}