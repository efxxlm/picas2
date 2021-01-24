import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-dialog-subsanacion',
  templateUrl: './dialog-subsanacion.component.html',
  styleUrls: ['./dialog-subsanacion.component.scss']
})
export class DialogSubsanacionComponent implements OnInit {

  constructor(public matDialogRef: MatDialogRef<DialogSubsanacionComponent>, @Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit(): void {
  }
  close() {
    this.matDialogRef.close('aceptado');
  }
}
