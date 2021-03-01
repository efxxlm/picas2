import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ActaInicioConstruccionService } from 'src/app/core/_services/actaInicioConstruccion/acta-inicio-construccion.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-dialog-cargar-acta-suscrita-const',
  templateUrl: './dialog-cargar-acta-suscrita-const.component.html',
  styleUrls: ['./dialog-cargar-acta-suscrita-const.component.scss']
})
export class DialogCargarActaSuscritaConstComponent implements OnInit {
  boton:string="Cargar";
  archivo: string;
  fileListaProyectos: FormControl;
  maxDate: Date;
  maxDate2: Date;


  public idContrato;
  public idRol;
  public contratacionId;
  public fechaTramite;
  public tipoContratoCodigo;
  public estadoDocumentoCodigo;
  public fechaEnvioFirma;
  public fechaFirmaFiduciaria;
  public numContrato;
  public fechaFirmaContrato;
  public fechaActaInicioFase1;
  public fechaTerminacion;
  public plazoFase1PreMeses;
  public plazoFase1PreDias;
  public plazoFase2ConstruccionMeses;
  public plazoFase2ConstruccionDias;
  public observaciones;
  public rutaDocumento;

  fechaSesionString: string;
  fechaSesion: Date;

  fechaSesionString2: string;
  fechaSesion2: Date;

  addressForm = new FormGroup({
    fechaFirmaContratistaObra: new FormControl(),
    fechaFirmaContratistaInterventoria: new FormControl(),
    documentoFile: new FormControl()
  })
  
  public fechaFirmaContratistaObra;
  public fechaFirmaContratistaInterventoria;

  public fecha1Titulo;
  public fecha2Titulo;

  esRojo: boolean = false;
  estaEditando = false;
  constructor(private router: Router,public dialog: MatDialog, public matDialogRef: MatDialogRef<DialogCargarActaSuscritaConstComponent>, @Inject(MAT_DIALOG_DATA) public data: any, private services: ActaInicioConstruccionService) {
    this.declararInputFile();
    this.maxDate = new Date();
    this.maxDate2 = new Date();
    if(data.id != undefined){
      this.idContrato = data.id;
    }
    if(data.idRol != undefined){
      this.idRol = data.idRol;
    }
    if(data.numContrato != undefined){
      this.numContrato = data.numContrato;
    }
    if(data.fecha1Titulo != undefined){
      this.fecha1Titulo = data.fecha1Titulo;
    }
    if(data.fecha2Titulo != undefined){
      this.fecha2Titulo = data.fecha2Titulo;
    }
  }

  ngOnInit(): void {

  }

  private declararInputFile() {
    this.fileListaProyectos = new FormControl('', [Validators.required]);
  }
  openDialog(modalTitle: string, modalText: string) {
    let dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '25em',
      data: { modalTitle, modalText }
    });
  }
  fileName(event: any) {
    if (event.target.files.length > 0) {
      const file = event.target.files[0];
      this.archivo = event.target.files[0].name;
      this.addressForm.patchValue({
        documentoFile: file
      });
    }
  }
  cargarActa(){
    this.estaEditando = true
    const inputNode: any = document.getElementById('file');
    this.archivo = inputNode.files[0].name;
    this.fechaSesion = new Date(this.addressForm.value.fechaFirmaContratistaObra);
    this.fechaSesionString = `${this.fechaSesion.getFullYear()}-${this.fechaSesion.getMonth() + 1}-${this.fechaSesion.getDate()}`;
    this.fechaSesion2 = new Date(this.addressForm.value.fechaFirmaContratistaInterventoria);
    this.fechaSesionString2 = `${this.fechaSesion2.getFullYear()}-${this.fechaSesion2.getMonth() + 1}-${this.fechaSesion2.getDate()}`;

    let pFile = inputNode.files[0];
    pFile = pFile.name.split('.');
    pFile = pFile[pFile.length - 1];

    if (pFile === 'pdf') {
      if (this.fechaSesionString == 'NaN-NaN-NaN' || this.fechaSesionString2 == 'NaN-NaN-NaN' || this.archivo == undefined) {
        this.openDialog('', '<b>Falta registrar información.</b>');
        this.esRojo = true;
      }
      else{
      this.services.EditCargarActaSuscritaContrato(this.idContrato,this.fechaSesionString,this.fechaSesionString2,inputNode.files[0],"usr3").subscribe(data=>{
        if(data.isSuccessful==true){
          this.openDialog('', `<b>${data.message}</b>`);
          this.close();
          this.services.EnviarCorreoSupervisorContratista(this.idContrato,this.idRol).subscribe(resp=>{
  
          });
        }
        else{
          this.openDialog('', `<b>${data.message}</b>`);
        }
      });
    }
  } else {
    this.openDialog('', '<b>El tipo de archivo que esta intentando cargar no es permitido en la plataforma.<br>El tipo de documento soportado es .pdf</b>');
    return false;
  }

  }
  close(){
    this.matDialogRef.close('aceptado');
}

}
