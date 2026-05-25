using System.Windows.Forms;
using LocationLibrary;

public static class EditObjectForm
{
    public static Place EditObject(Place original)
    {
        using (var dialog = new Lab16.ObjectDialog(original))
        {
            if (dialog.ShowDialog() == DialogResult.OK)
                return dialog.ResultObject;
            return null;
        }
    }
}