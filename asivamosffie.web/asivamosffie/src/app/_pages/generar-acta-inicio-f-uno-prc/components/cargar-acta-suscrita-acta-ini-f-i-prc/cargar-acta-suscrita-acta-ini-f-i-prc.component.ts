import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { Contrato, ContratoParaActa, EditContrato, GestionarActPreConstrFUnoService } from 'src/app/core/_services/GestionarActPreConstrFUno/gestionar-act-pre-constr-funo.service';

@Component({
  selector: 'app-cargar-acta-suscrita-acta-ini-f-i-prc',
  templateUrl: './cargar-acta-suscrita-acta-ini-f-i-prc.component.html',
  styleUrls: ['./cargar-acta-suscrita-acta-ini-f-i-prc.component.scss']
})
export class CargarActaSuscritaActaIniFIPreconstruccionComponent implements OnInit {
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
  public fechaFirmaContratista;
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
  idRol: any;
  fecha1Titulo: any;
  fecha2Titulo: any;


  constructor(private router: Router,public dialog: MatDialog, public matDialogRef: MatDialogRef<CargarActaSuscritaActaIniFIPreconstruccionComponent>, @Inject(MAT_DIALOG_DATA) public data: any, private service: GestionarActPreConstrFUnoService) { 
    this.declararInputFile();
    this.maxDate = new Date();
    this.maxDate2 = new Date();
    if (data.id != undefined) {
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
    this.loadData(this.idContrato);
  }
  loadData(id){
    this.service.GetContratoByContratoId(id).subscribe(data=>{
      this.cargarDataParaInsercion(data);
    });
    this.idContrato = id;
  }
  cargarDataParaInsercion(data){
    this.numContrato = data.numeroContrato;
    this.fechaFirmaContrato = data.fechaFirmaContrato;
    this.contratacionId = data.contratacionId;
    this.fechaTramite = data.fechaTramite;
    this.tipoContratoCodigo = data.tipoContratoCodigo;
    this.estadoDocumentoCodigo = data.estadoDocumentoCodigo;
    this.fechaEnvioFirma = data.fechaEnvioFirma;
    this.fechaFirmaContratista = data.fechaFirmaContratista;
    this.fechaFirmaFiduciaria = data.fechaFirmaFiduciaria;
    this.fechaActaInicioFase1 = data.fechaActaInicioFase1;
    this.fechaTerminacion = data.fechaTerminacion;
    this.plazoFase1PreMeses = data.plazoFase1PreMeses;
    this.plazoFase1PreDias = data.plazoFase1PreDias;
    this.plazoFase2ConstruccionMeses = data.plazoFase2ConstruccionMeses;
    this.plazoFase2ConstruccionDias = data.plazoFase2ConstruccionDias;
    this.observaciones = data.observaciones;
    this.rutaDocumento = "https://meet.google.com/pwd-mgvi-egp?pli=1&authuser=1";
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

    const arraycontrato: ContratoParaActa ={
      contratoId: this.idContrato,
      contratacionId: this.contratacionId,
      fechaTramite: this.fechaTramite,
      tipoContratoCodigo: this.tipoContratoCodigo,
      numeroContrato: this.numContrato,
      estadoDocumentoCodigo: this.estadoDocumentoCodigo,
      estado: false,
      fechaEnvioFirma: this.fechaEnvioFirma,
      fechaFirmaContratista: this.fechaFirmaContratista,
      fechaFirmaFiduciaria: this.fechaFirmaFiduciaria,
      fechaFirmaContrato: this.fechaFirmaContrato,
      fechaActaInicioFase1: this.fechaActaInicioFase1,
      fechaTerminacion: this.fechaTerminacion,
      fechaFirmaActaContratista: this.maxDate,
      fechaFirmaActaContratistaInterventoria: this.maxDate2,
      plazoFase1PreMeses: this.plazoFase1PreMeses,
      plazoFase1PreDias: this.plazoFase1PreDias,
      plazoFase2ConstruccionMeses: this.plazoFase2ConstruccionMeses,
      plazoFase2ConstruccionDias: this.plazoFase2ConstruccionDias,
      observaciones: this.observaciones,
      conObervacionesActa: false,
      registroCompleto: false,
      contratoConstruccion: [],
      contratoObservacion: [],
      contratoPerfil: [],
      contratoPoliza: []
    };
    this.service.LoadActa(arraycontrato,inputNode.files[0],this.rutaDocumento,this.rutaDocumento).subscribe(data=>{
      if(data.isSuccessful==true){
        this.openDialog('La información ha sido guardada exitosamente.', "");
        this.close();
      } 
      else{
        this.openDialog(data.message,"");
      }
    });
  }
  close(){
    this.matDialogRef.close('aceptado');
}
}
