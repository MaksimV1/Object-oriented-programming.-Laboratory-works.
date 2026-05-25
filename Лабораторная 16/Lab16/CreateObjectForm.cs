using System.Windows.Forms;
using LocationLibrary;

public static class CreateObjectForm
{
    public static Place CreateObject()
    {
        using (var dialog = new Lab16.ObjectDialog())
        {
            if (dialog.ShowDialog() == DialogResult.OK)
                return dialog.ResultObject;
            return null;
        }
    }
}