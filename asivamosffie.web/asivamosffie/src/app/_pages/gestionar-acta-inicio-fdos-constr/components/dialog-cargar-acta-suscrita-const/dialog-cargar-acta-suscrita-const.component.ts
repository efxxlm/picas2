import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ActBeginService } from 'src/app/core/_services/actBegin/act-begin.service';
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

  public fechaFirmaContratistaObra;
  public fechaFirmaContratistaInterventoria;

  constructor(private router: Router,public dialog: MatDialog, public matDialogRef: MatDialogRef<DialogCargarActaSuscritaConstComponent>, @Inject(MAT_DIALOG_DATA) public data: any, private services: ActBeginService) { 
    this.declararInputFile();
    this.maxDate = new Date();
    this.maxDate2 = new Date();
    if(data.id != undefined){
      this.idContrato = data.id;
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
  fileName() {
    const inputNode: any = document.getElementById('file');
    this.archivo = inputNode.files[0].name;
  }
  cargarActa(){
    const inputNode: any = document.getElementById('file');
    this.archivo = inputNode.files[0].name;
    this.services.EditCargarActaSuscritaContrato(this.idContrato,this.fechaFirmaContratistaObra,this.fechaFirmaContratistaInterventoria,inputNode.files[0],"usr3").subscribe(data=>{
      if(data.code=="200"){
        this.openDialog(data.message,"");
        this.close();
      }
      else{
        this.openDialog(data.message,"");
      }
    });
  }
  close(){
    this.matDialogRef.close('cancel');
}

}
