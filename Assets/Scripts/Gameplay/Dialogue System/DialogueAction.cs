using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public abstract class DialogueActionBase
{
    public virtual void DoAction()
    {

    }
}

[Serializable]
public partial class DialogueAction : DialogueActionBase
{

}
