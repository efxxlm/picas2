import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { DefensaJudicial } from 'src/app/core/_services/defensaJudicial/defensa-judicial.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-form-urlsoporte-dj',
  templateUrl: './form-urlsoporte-dj.component.html',
  styleUrls: ['./form-urlsoporte-dj.component.scss']
})
export class FormUrlsoporteDjComponent implements OnInit {

 
  addressForm = this.fb.group({
    urlSoporte: [null, Validators.required]
  });
  constructor(  private fb: FormBuilder, public dialog: MatDialog) { }

  ngOnInit(): void {
  }

  @Input() legitimacion:boolean;
  @Input() tipoProceso:string;
  @Input() defensaJudicial:DefensaJudicial;
  cargarRegistro() {
    //this.ngOnInit().then(() => {
      console.log("form");
      console.log(this.defensaJudicial);
      console.log(this.legitimacion);
      console.log(this.tipoProceso);      
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  onSubmit() {
    console.log(this.addressForm.value);
    this.openDialog('', 'La información ha sido guardada exitosamente.');
  }
}