using System.Collections.Generic;

namespace PolicyExample.GraphQL.DTO {

    /// <summary>
    /// A node in a logic tree. If ParentID is empty, than it consider as root.
    /// </summary>
    public class LogicNode {
   
      
      public string Id { get; set; }
    
      
      public string Name { get; set; }
    
      
      public Script Script { get; set; }
    
      
      public LogicNode Parent { get; set; }
    
      
      public List<LogicNode> Children { get; set; }
      
    }
    
    
 

    
    
 

    
    
 

    
    
 

    
    
 

    
    
 

    

 

    
    
 

    

 

    
    
 

    
    
 

    
    
 

    
    
 

    
    
 

    
    
 

    


 

    
    
 

    
  }
  

