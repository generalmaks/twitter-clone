namespace TwitterCloneBack.Orchestrator;

public abstract class OrchestratorException : Exception
{
    protected OrchestratorException(string message) : base(message)
    {
    }
}

public class NotFoundException : OrchestratorException
{
    public NotFoundException(string message) : base(message)
    {
    }
}

public class InvalidArgumentException : OrchestratorException
{
    public InvalidArgumentException(string message) : base(message)
    {
    }
}

public class ForbiddenException : OrchestratorException
{
    public ForbiddenException(string message) : base(message)
    {
    }
}