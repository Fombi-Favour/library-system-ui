using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibMS;

namespace LibMS
{
    public abstract class ActivateMessageBox
    {
        public static DialogResult Show(string text)
        {
            DialogResult result;
            using (var msgForm = new MessageBoxForm(text))
                result = msgForm.ShowDialog();
            return result;
        }

        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            DialogResult result;
            using (var msgForm = new MessageBoxForm(text, caption, buttons, icon))
                result = msgForm.ShowDialog();
            return result;
        }

        /*-> IWin32Window Owner:
            *      Displays a message box in front of the specified object and with the other specified parameters.
            *      An implementation of IWin32Window that will own the modal dialog box.*/

        public static DialogResult Show(IWin32Window owner, string text)
        {
            DialogResult result;
            using (var msgForm = new MessageBoxForm(text))
                result = msgForm.ShowDialog(owner);
            return result;
        }

        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            DialogResult result;
            using (var msgForm = new MessageBoxForm(text, caption, buttons, icon))
                result = msgForm.ShowDialog(owner);
            return result;
        }
    }
}
