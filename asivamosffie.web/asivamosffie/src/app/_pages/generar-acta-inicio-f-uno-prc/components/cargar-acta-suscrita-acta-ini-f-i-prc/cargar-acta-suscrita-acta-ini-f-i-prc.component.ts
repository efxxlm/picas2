import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { GestionarActPreConstrFUnoService } from 'src/app/core/_services/GestionarActPreConstrFUno/gestionar-act-pre-constr-funo.service';

@Component({
  selector: 'app-cargar-acta-suscrita-acta-ini-f-i-prc',
  templateUrl: './cargar-acta-suscrita-acta-ini-f-i-prc.component.html',
  styleUrls: ['./cargar-acta-suscrita-acta-ini-f-i-prc.component.scss']
})
export class CargarActaSuscritaActaIniFIPreconstruccionComponent implements OnInit {
  boton: string = "Cargar";
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

  public fechaFirmaContratistaObra;
  public fechaFirmaContratistaInterventoria;

  public fecha1Titulo;
  public fecha2Titulo;

  prueba: any;

  constructor(private router: Router, public dialog: MatDialog, public matDialogRef: MatDialogRef<CargarActaSuscritaActaIniFIPreconstruccionComponent>, @Inject(MAT_DIALOG_DATA) public data: any, private service: GestionarActPreConstrFUnoService) {
    this.declararInputFile();
    this.maxDate = new Date();
    this.maxDate2 = new Date();
    if (data.id != undefined) {
      this.idContrato = data.id;
    }
    if (data.idRol != undefined) {
      this.idRol = data.idRol;
    }
    if (data.numContrato != undefined) {
      this.numContrato = data.numContrato;
    }
    if (data.fecha1Titulo != undefined) {
      this.fecha1Titulo = data.fecha1Titulo;
    }
    if (data.fecha2Titulo != undefined) {
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
  fileName() {
    const inputNode: any = document.getElementById('file');
    this.archivo = inputNode.files[0].name;
  }
  cargarActa() {
    const pContrato = new FormData();
    const inputNode: any = document.getElementById('file');
    //this.archivo = inputNode.files[0].name;
    this.fechaSesion = new Date(this.fechaFirmaContratistaObra);
    this.fechaSesionString = `${this.fechaSesion.getFullYear()}-${this.fechaSesion.getMonth() + 1}-${this.fechaSesion.getDate()}`;
    this.fechaSesion2 = new Date(this.fechaFirmaContratistaInterventoria);
    this.fechaSesionString2 = `${this.fechaSesion2.getFullYear()}-${this.fechaSesion2.getMonth() + 1}-${this.fechaSesion2.getDate()}`;
    pContrato.append('ContratoId', this.idContrato);
    pContrato.append('FechaActaInicioFase1', this.fechaSesionString);
    pContrato.append('FechaTerminacion', this.fechaSesionString2);
    pContrato.append('pFile', inputNode.files[0]);  
    if(this.fechaSesionString=='NaN-NaN-NaN' || this.fechaSesionString2=='NaN-NaN-NaN' || this.archivo==undefined){
      this.openDialog('', '<b>Falta registrar información.</b>');
    }
    else{
      this.service.LoadActa(pContrato).subscribe((data: any) => {
        if (data.code == "200") {
          this.openDialog('', '<b>La información ha sido guardada exitosamente.</b>');
          this.close();
        }
        else {
          this.openDialog("", data.message);
        }
      });
    }

  }
  close() {
    this.matDialogRef.close('aceptado');
  }
}
