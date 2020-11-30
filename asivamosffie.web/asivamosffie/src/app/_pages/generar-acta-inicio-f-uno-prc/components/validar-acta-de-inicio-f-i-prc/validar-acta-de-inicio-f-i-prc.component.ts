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
  constructor(private activatedRoute: ActivatedRoute, private service: GestionarActPreConstrFUnoService, private router: Router,public dialog: MatDialog, private fb: FormBuilder) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(param => {
      this.loadData(param.id);
      this.contratoId = param.id;
    });
  }
  loadData(id){
    this.service.GetContratoByContratoId(id).subscribe((data:any)=>{
      this.cargarDataParaInsercion(data);
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
  cargarDataParaInsercion(data){
    this.numContrato = data.numeroContrato;
    this.fechaAprobacionRequisitos = data.fechaAprobacionRequisitos;
    this.fechaFirmaContrato = data.fechaFirmaContrato;
    this.contratacionId = data.contratacionId;
    this.fechaTramite = data.fechaTramite;
    this.tipoContratoCodigo = data.tipoContratoCodigo;
    this.estadoDocumentoCodigo = data.estadoDocumentoCodigo;
    this.fechaEnvioFirma = data.fechaEnvioFirma;
    this.fechaFirmaContratista = data.fechaFirmaContratista;
    this.fechaFirmaFiduciaria = data.fechaFirmaFiduciaria;
    this.numDRP = data.contratacion.disponibilidadPresupuestal[0].numeroDrp;
    this.fechaDRP = data.contratacion.disponibilidadPresupuestal[0].fechaCreacion;
    this.objeto = data.objeto;
    this.valorIni = data.valor;
    this.nitContratistaInterventoria = data.contratacion.contratista.numeroIdentificacion;
    this.nomContratista = data.contratacion.contratista.nombre;
    this.fechaActaFase1Prc = data.fechaActaInicioFase1;
  }
}
