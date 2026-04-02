public class Inverter : Node
{
   public Inverter(string nodeName)
   {
       NodeName = nodeName;
   }
   
   public override NodeStatus Process()
   {
       var childStatus = Children[CurrentChild].Process();
        
       switch (childStatus)
       {
           case NodeStatus.Running:
               return NodeStatus.Running;
           case NodeStatus.Failure:
               return NodeStatus.Success;
       }

         return NodeStatus.Failure;
   }
}