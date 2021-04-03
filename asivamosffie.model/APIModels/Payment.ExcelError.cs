namespace asivamosffie.model.APIModels
{
    public class ExcelError
    {
        public int Row { get; set; }
        public int Column { get; set; }        
        public string Error { get; set; }

        public ExcelError(int row, int col, string error)
        {
            this.Column = col;
            this.Row = row;
            this.Error = error;
        }

    }
}
