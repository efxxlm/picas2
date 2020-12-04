import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { GestionarActPreConstrFUnoService } from 'src/app/core/_services/GestionarActPreConstrFUno/gestionar-act-pre-constr-funo.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-validar-acta-de-inicio-f-i-prc',
  templateUrl: './validar-acta-de-inicio-f-i-prc.component.html',
  styleUrls: ['./validar-acta-de-inicio-f-i-prc.component.scss']
})
export class ValidarActaDeInicioFIPreconstruccionComponent implements OnInit {
  
  dataDialog: {
    modalTitle: string,
    modalText: string
  };
  contratoId: any;
  nomContratista: any;
  numContrato: string;
  fechaFirmaContrato: Date;
  fechaActaFase1Prc: Date;
  fechaTermPrevista: Date;
  diasFase1: number;
  mesesFase1: number;
  diasFase2: number;
  mesesFase2: number;
  observaciones: string;
  fechaAprobacionRequisitos: any;
  contratacionId: any;
  fechaTramite: any;
  tipoContratoCodigo: any;
  estadoDocumentoCodigo: any;
  fechaEnvioFirma: any;
  fechaFirmaContratista: any;
  fechaFirmaFiduciaria: any;
  numDRP: any;
  fechaDRP: any;
  objeto: any;
  valorIni: any;
  nitContratistaInterventoria: any;
  public mesPlazoIni: number = 10;
  public diasPlazoIni: number = 25;
  fechaAprobGarantiaPoliza: any;
  vigenciaContrato: any;
  valorFUno: any;
  valorFDos: any;
  nomEntidadContratistaIntervn: any;
  numIdContratistaObra: any;
  numIdRepresentanteLegal: any;
  nomRepresentanteLegal: any;
  tipoProponente: any;
  rolAsignado: any;
  opcion: number;
  numIdentifiacionSupervisor: any;
  nomSupervisor: string;
  dataSupervisor: boolean;
  tipoCodigo: any;
  dataElements: any;
  tieneObservacionesBool: any;
  observacionesUltimas: any;
  fechaCreacionObs: any;
  constructor(private activatedRoute: ActivatedRoute, private service: GestionarActPreConstrFUnoService, private router: Router,public dialog: MatDialog, private fb: FormBuilder) { }

  ngOnInit(): void {
    this.cargarRol();
    this.activatedRoute.params.subscribe(param => {
      this.loadData(param.id);
      this.loadService(param.id)
      this.contratoId = param.id;
    });
  }
  cargarRol() {
    this.rolAsignado = JSON.parse(localStorage.getItem("actualUser")).rol[0].perfilId;
    if (this.rolAsignado == 11) {
      this.opcion = 1;
    }
    else {
      this.opcion = 2;
    }
  }
  loadData(id){
    this.service.GetContratoByContratoId(id).subscribe((data:any)=>{
      this.cargarDataParaInsercion(data);
      this.fechaActaFase1Prc = data.fechaActaInicioFase1;
      this.numContrato = data.numeroContrato;
      this.fechaFirmaContrato = data.fechaFirmaContrato;
      this.fechaTermPrevista = data.fechaTerminacion;
      this.diasFase1 = data.plazoFase1PreDias;
      this.mesesFase1 = data.plazoFase1PreMeses;
      this.diasFase2 = data.plazoFase2ConstruccionDias;
      this.mesesFase2 = data.plazoFase2ConstruccionMeses;
      if(data.observaciones==undefined || data.observaciones==null || data.observaciones=="") {
        this.observaciones = "-----";
      }
      else{
        this.observaciones = data.observaciones;
      }
    });
  }
  loadService(id){
    this.service.GetListContratoObservacionByContratoId(id).subscribe((data:any)=>{
      this.dataElements = data;
      for(let i=0; i<data.length;i++){ 
        if(data[i].esSupervision==false){
          this.tieneObservacionesBool = this.dataElements[i].esActaFase1;
          this.observacionesUltimas = this.dataElements[i].observaciones;
          this.fechaCreacionObs = this.dataElements[i].fechaCreacion;
        }
      }
    });
  }
  cargarDataParaInsercion(data){
    this.numContrato = data.numeroContrato;
    this.fechaAprobacionRequisitos = data.fechaAprobacionRequisitosSupervisor;
    this.fechaFirmaContrato = data.fechaFirmaContrato;
    this.contratacionId = data.contratacionId;
    this.fechaTramite = data.fechaTramite;
    this.tipoContratoCodigo = data.tipoContratoCodigo;
    this.estadoDocumentoCodigo = data.estadoDocumentoCodigo;
    this.fechaEnvioFirma = data.fechaEnvioFirma;
    this.fechaFirmaContratista = data.fechaFirmaContratista;
    this.fechaFirmaFiduciaria = data.fechaFirmaFiduciaria;
    this.numDRP = data.contratacion.disponibilidadPresupuestal[0].numeroDrp;
    this.fechaDRP = data.contratacion.disponibilidadPresupuestal[0].fechaDrp;
    this.objeto = data.contratacion.disponibilidadPresupuestal[0].objeto;
    this.valorIni = data.contratacion.disponibilidadPresupuestal[0].valorSolicitud;
    this.numIdRepresentanteLegal = data.contratacion.contratista.representanteLegalNumeroIdentificacion;
    this.nomRepresentanteLegal = data.contratacion.contratista.representanteLegal;
    this.nitContratistaInterventoria = data.contratacion.contratista.numeroIdentificacion;
    this.fechaAprobGarantiaPoliza = data.contratoPoliza[0].fechaAprobacion;
    this.vigenciaContrato = data.fechaTramite;
    this.valorFUno = data.valorFase1;
    this.valorFDos = data.valorFase2;
    this.nomEntidadContratistaIntervn = data.contratacion.contratista.nombre;
    this.numIdContratistaObra = data.contratacion.contratista.representanteLegalNumeroIdentificacion
    this.mesPlazoIni= data.contratacion.disponibilidadPresupuestal[0].plazoMeses;
    this.diasPlazoIni= data.contratacion.disponibilidadPresupuestal[0].plazoDias;
    this.tipoProponente = data.contratacion.contratista.tipoProponenteCodigo;
    this.tipoCodigo = data.contratacion.tipoSolicitudCodigo;
    this.numIdentifiacionSupervisor = data.usuarioInterventoria.numeroIdentificacion;
    this.nomSupervisor = data.usuarioInterventoria.nombres+" "+data.usuarioInterventoria.apellidos;
    if(this.opcion == 1){
      this.dataSupervisor = true;
      this.numIdentifiacionSupervisor = data.usuarioInterventoria.numeroIdentificacion;
      this.nomSupervisor = data.usuarioInterventoria.nombres+" "+data.usuarioInterventoria.apellidos;
    }
  }
}
