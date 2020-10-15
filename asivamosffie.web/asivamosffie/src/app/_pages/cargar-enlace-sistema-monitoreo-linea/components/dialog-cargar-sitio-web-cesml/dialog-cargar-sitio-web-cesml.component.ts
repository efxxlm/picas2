import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-dialog-cargar-sitio-web-cesml',
  templateUrl: './dialog-cargar-sitio-web-cesml.component.html',
  styleUrls: ['./dialog-cargar-sitio-web-cesml.component.scss']
})
export class DialogCargarSitioWebCesmlComponent implements OnInit {
  public idProyecto;
  public llaveMen;
  public departamento;
  public municipio;
  public instEdu;
  public sede;

  addressForm = this.fb.group({
    observaciones: [null, Validators.required]
  });

  constructor(private fb: FormBuilder, public dialog: MatDialog, public matDialogRef: MatDialogRef<DialogCargarSitioWebCesmlComponent>, @Inject(MAT_DIALOG_DATA) public data: any) { 
    if (data.id != undefined) {
      this.idProyecto = data.id;
    }
    if(data.llaveMen !=undefined){
      this.llaveMen = data.llaveMen;
    }
    if(data.departamento !=undefined){
      this.departamento = data.departamento;
    }
    if(data.municipio !=undefined){
      this.municipio = data.municipio;
    }
    if(data.instEdu !=undefined){
      this.instEdu = data.instEdu;
    }
    if(data.sede !=undefined){
      this.sede = data.sede;
    }
  }

  ngOnInit(): void {
  }
  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  textoLimpio(texto: string) {
    const textolimpio = texto.replace(/<[^>]*>/g, '');
    return textolimpio.length;
  }
  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  };
  onSubmit(){
    this.openDialog('La información ha sido guardada exitosamente.', '');
  }
}
