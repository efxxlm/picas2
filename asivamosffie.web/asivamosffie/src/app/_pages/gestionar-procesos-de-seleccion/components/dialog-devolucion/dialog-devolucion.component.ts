import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-dialog-devolucion',
  templateUrl: './dialog-devolucion.component.html',
  styleUrls: ['./dialog-devolucion.component.scss']
})
export class DialogDevolucionComponent implements OnInit {

  constructor(
              public dialogRef: MatDialogRef<DialogDevolucionComponent>,
              @Inject(MAT_DIALOG_DATA) public data
  ) { }

  ngOnInit(): void {
    console.log(this.data)
  }

}
