import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subject } from 'rxjs';
import { MonitoringURLService } from 'src/app/core/_services/monitoringURL/monitoring-url.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-dialog-cargar-sitio-web-cesml',
  templateUrl: './dialog-cargar-sitio-web-cesml.component.html',
  styleUrls: ['./dialog-cargar-sitio-web-cesml.component.scss']
})
export class DialogCargarSitioWebCesmlComponent implements OnInit {

  public title;
  public idProyecto;
  public llaveMen;
  public departamento;
  public municipio;
  public instEdu;
  public sede;
  public web;

  addressForm = this.fb.group({
    urlMonitoreo: [null, Validators.required]
  });

  constructor(private services: MonitoringURLService, private fb: FormBuilder, public dialog: MatDialog, public matDialogRef: MatDialogRef<DialogCargarSitioWebCesmlComponent>, @Inject(MAT_DIALOG_DATA) public data: any) { 
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
    if(data.web !=undefined){
      this.web = data.web;
    }
    if(data.title !=undefined){
      this.title = data.title;
    }
  }

  ngOnInit(): void {
    this.addressForm.get('urlMonitoreo').setValue(this.web);
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
    this.services.EditarURLMonitoreo(this.idProyecto,this.addressForm.value.urlMonitoreo).subscribe(resp=>{
      if(resp.code=="200"){
        this.openDialog( '', `<b>${ resp.message }</b>` );
        this.close();
      }
      else{
        this.openDialog( '', `<b>${ resp.message }</b>` );
      }
    });
  }
  close(){
    this.matDialogRef.close('aceptado');
  }
}
