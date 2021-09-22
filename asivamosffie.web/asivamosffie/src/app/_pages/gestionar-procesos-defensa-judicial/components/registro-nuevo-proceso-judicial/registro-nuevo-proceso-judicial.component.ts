import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { forkJoin } from 'rxjs';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { DefensaJudicial, DefensaJudicialService } from 'src/app/core/_services/defensaJudicial/defensa-judicial.service';

@Component({
  selector: 'app-registro-nuevo-proceso-judicial',
  templateUrl: './registro-nuevo-proceso-judicial.component.html',
  styleUrls: ['./registro-nuevo-proceso-judicial.component.scss']
})
export class RegistroNuevoProcesoJudicialComponent implements OnInit {
  addressForm = this.fb.group({
    legitimacionActiva: [null, Validators.required],
    tipoProceso: [null, Validators.required],
  });
  tipoProcesoArray = [];
  controlJudicialId: any;
  defensaJudicial: DefensaJudicial={};
  contratos_class: number=0;
  detalle_class:number=0;
  convocados_class:number=0;
  demandantes_class:number=0;
  demandado_class_pasiva: number =0;
  convocado_class_pasiva: number =0;
  soporte_class:number=0;
  ficha_class:number=3;
  textCabecera: string;
  estaEditando = false;
  //tieneDemanda:boolean;
  tieneDemanda: Subscription;
  numAcordTieneDemanda:any;
  constructor(private fb: FormBuilder, public dialog: MatDialog,
    public commonServices: CommonService,
    public judicialServices:DefensaJudicialService,
    private activatedRoute: ActivatedRoute,) { }

    async editMode(){
      this.estaEditando = true;
      this.addressForm.markAllAsTouched();
      this.cargarRegistro().then(() =>
      {

        this.addressForm.get("legitimacionActiva").setValue(this.defensaJudicial.esLegitimacionActiva);
        this.addressForm.get("tipoProceso").setValue(this.defensaJudicial.tipoProcesoCodigo);

      });

    }

  async cargarRegistro() {
    return new Promise( resolve => {

      forkJoin([
        this.judicialServices.GetDefensaJudicialById(this.controlJudicialId)
      ]).subscribe( proceso => {
          this.defensaJudicial=proceso[0];
          this.textCabecera="Ver detalle/Editar proceso de defensa judicial "+this.defensaJudicial.numeroProceso;
          console.log(this.defensaJudicial);
          this.contratos_class=this.estaIncompletocontratos(this.defensaJudicial);
          this.detalle_class=this.estaIncompletodetalle(this.defensaJudicial);
          this.convocados_class=this.estaIncompletoconvocados(this.defensaJudicial);
          this.demandantes_class= this.estaIncompletodemandanteconvocante(this.defensaJudicial);
          this.demandado_class_pasiva = this.estaIncompletoDemandadoPasiva(this.defensaJudicial);
          this.convocado_class_pasiva = this.estaIncompletoConvocadoPasiva(this.defensaJudicial);
          console.log(this.convocados_class);
          this.soporte_class=this.defensaJudicial.urlSoporteProceso==null || this.defensaJudicial.urlSoporteProceso== ""?0:2;
          this.ficha_class=this.estaIncompletoficha(this.defensaJudicial);
          setTimeout(() => { resolve(''); },1000)
      });
    });


  }
  estaIncompletoficha(defensaJudicial: DefensaJudicial): number {
    //'en-alerta':ficha_class===3,'sin-diligenciar':ficha_class===0,'en-proceso':ficha_class===1,'completo':ficha_class===2
    let retorno:number=0;
    if(defensaJudicial.fichaEstudio.length>0){
      if(defensaJudicial.fichaEstudio[0].esCompleto == null){
        if(this.contratos_class === 2 && this.detalle_class === 2 && this.soporte_class=== 2
          && ((this.convocados_class === 2 || this.demandantes_class===2))
          && ((this.numAcordTieneDemanda > 0 ? (this.demandado_class_pasiva === 2 || this.convocado_class_pasiva===2) : (this.numAcordTieneDemanda == undefined || this.numAcordTieneDemanda == null || this.numAcordTieneDemanda == 0) ))
        ){
            retorno=0;
        }else{
          retorno=3;
        }
      }else if(!defensaJudicial.fichaEstudio[0].esCompleto){
        if((defensaJudicial.fichaEstudio[0].antecedentes==null || defensaJudicial.fichaEstudio[0].antecedentes == '')
          && (defensaJudicial.fichaEstudio[0].hechosRelevantes==null || defensaJudicial.fichaEstudio[0].hechosRelevantes == '')
          && (defensaJudicial.fichaEstudio[0].jurisprudenciaDoctrina==null || defensaJudicial.fichaEstudio[0].jurisprudenciaDoctrina == '')
          && (defensaJudicial.fichaEstudio[0].decisionComiteDirectrices==null || defensaJudicial.fichaEstudio[0].decisionComiteDirectrices == '')
          && (defensaJudicial.fichaEstudio[0].analisisJuridico==null || defensaJudicial.fichaEstudio[0].analisisJuridico == '')
          && (defensaJudicial.fichaEstudio[0].recomendaciones==null || defensaJudicial.fichaEstudio[0].recomendaciones == '')
          && defensaJudicial.fichaEstudio[0].esPresentadoAnteComiteFfie== null
          && defensaJudicial.fichaEstudio[0].esAprobadoAperturaProceso==null
          && (defensaJudicial.fichaEstudio[0].tipoActuacionCodigo==null || defensaJudicial.fichaEstudio[0].tipoActuacionCodigo == '')
          && defensaJudicial.fichaEstudio[0].esActuacionTramiteComite==null
          && (defensaJudicial.fichaEstudio[0].rutaSoporte==null || defensaJudicial.fichaEstudio[0].rutaSoporte == '')){
            retorno = 0;
          }else{
            retorno = 1;
          }
      }else{
        retorno = 2;
      }
    }else{
      if(this.contratos_class === 2 && this.detalle_class === 2 && this.soporte_class=== 2
        && ((this.convocados_class === 2 || this.demandantes_class===2))
        && ((this.numAcordTieneDemanda > 0 ? (this.demandado_class_pasiva === 2 || this.convocado_class_pasiva===2) : (this.numAcordTieneDemanda == undefined || this.numAcordTieneDemanda == null || this.numAcordTieneDemanda == 0) ))
      ){
          retorno=0;
      }else{
        retorno=3;
      }
    }
    return retorno;
  }
  estaIncompletoconvocados(defensaJudicial: DefensaJudicial): number {
    let retorno:number=0;
    //sin-diligenciar:retorno===0,'en-proceso':retorno===1,'completo':retorno===2
    if(defensaJudicial != null){
      let num_enproceso:number=0;
      let num_sindiligenciar:number=0;

      let num_convocados = defensaJudicial.numeroDemandados;// total de convocados
      let num_completo = 0; //almacena los registros que estan completos

      defensaJudicial.demandadoConvocado.forEach(element => {
          if( element.registroCompleto == null
            || (!element.registroCompleto
            && (element.nombre == null || element.nombre == '')
            && (element.tipoIdentificacionCodigo == null || element.tipoIdentificacionCodigo == '')
            && (element.numeroIdentificacion == null || element.numeroIdentificacion == '')
            && (element.direccion == null || element.direccion == '')
            && (element.email == null || element.email == '')
            )){
                num_sindiligenciar = num_sindiligenciar+1;
            }else if(!element.registroCompleto){
                num_enproceso = num_enproceso+1;
            }else if(element.registroCompleto){
              num_completo = num_completo+1;
            }
      });

      if(num_sindiligenciar>= num_convocados){
        retorno = 0;
      }else if(num_enproceso > 0 || (num_completo > 0 && num_completo< num_convocados) ){
        retorno = 1;
      }else if(num_completo >= num_convocados){
          retorno = 2;
      }

    }

    return retorno;
  }

  estaIncompletodemandanteconvocante(defensaJudicial: DefensaJudicial): number {
    let retorno:number=0;
    //sin-diligenciar:retorno===0,'en-proceso':retorno===1,'completo':retorno===2
    if(defensaJudicial != null){
      let num_enproceso:number=0;
      let num_sindiligenciar:number=0;

      let num_convocados = defensaJudicial.numeroDemandantes;// total de convocados
      let num_completo = 0; //almacena los registros que estan completos

      defensaJudicial.demandanteConvocante.forEach(element => {
          if( element.registroCompleto == null
            || (!element.registroCompleto
            && (element.nombre == null || element.nombre == '')
            && (element.tipoIdentificacionCodigo == null || element.tipoIdentificacionCodigo == '')
            && (element.numeroIdentificacion == null || element.numeroIdentificacion == '')
            && (element.direccion == null || element.direccion == '')
            && (element.email == null || element.email == '')
            )){
                num_sindiligenciar = num_sindiligenciar+1;
            }else if(!element.registroCompleto){
                num_enproceso = num_enproceso+1;
            }else if(element.registroCompleto){
              num_completo = num_completo+1;
            }
      });

      if(num_sindiligenciar>= num_convocados){
        retorno = 0;
      }else if(num_enproceso > 0 || (num_completo > 0 && num_completo< num_convocados) ){
        retorno = 1;
      }else if(num_completo >= num_convocados){
          retorno = 2;
      }

    }

    return retorno;
  }

  estaIncompletoDemandadoPasiva(defensaJudicial: DefensaJudicial): number {
    let retorno: number = 0;
    //sin-diligenciar:retorno===0,'en-proceso':retorno===1,'completo':retorno===2
    if (defensaJudicial != null) {
      let num_enproceso: number = 0;
      let num_sindiligenciar: number = 0;

      let num_convocados = defensaJudicial.numeroDemandantes;// total de convocados
      let num_completo = 0; //almacena los registros que estan completos
      defensaJudicial.demandadoConvocado.forEach(element => {
        if (element.esDemandado) {
          if (element.registroCompleto == null
            || (!element.registroCompleto
              && (element.nombre == null || element.nombre == '')
              && (element.tipoIdentificacionCodigo == null || element.tipoIdentificacionCodigo == '')
              && (element.numeroIdentificacion == null || element.numeroIdentificacion == '')
              && (element.direccion == null || element.direccion == '')
              && (element.email == null || element.email == '')
            )) {
            num_sindiligenciar = num_sindiligenciar + 1;
          } else if (!element.registroCompleto) {
            num_enproceso = num_enproceso + 1;
          } else if (element.registroCompleto) {
            num_completo = num_completo + 1;
          }
        }
      });
      if (num_sindiligenciar >= num_convocados) {
        retorno = 0;
      } else if (num_enproceso > 0 || (num_completo > 0 && num_completo < num_convocados)) {
        retorno = 1;
      } else if (num_completo >= num_convocados) {
        retorno = 2;
      }

    }

    return retorno;
  }

  estaIncompletoConvocadoPasiva(defensaJudicial: DefensaJudicial): number {
    let retorno: number = 0;
    //sin-diligenciar:retorno===0,'en-proceso':retorno===1,'completo':retorno===2
    if (defensaJudicial != null) {
      let num_enproceso: number = 0;
      let num_sindiligenciar: number = 0;

      let num_convocados = defensaJudicial.numeroDemandados;// total de convocados
      let num_completo = 0; //almacena los registros que estan completos
      defensaJudicial.demandadoConvocado.forEach(element => {
        if (element.esConvocado) {
          if (element.registroCompleto == null
            || (!element.registroCompleto
              && (element.nombre == null || element.nombre == '')
              && (element.tipoIdentificacionCodigo == null || element.tipoIdentificacionCodigo == '')
              && (element.numeroIdentificacion == null || element.numeroIdentificacion == '')
              && (element.direccion == null || element.direccion == '')
              && (element.email == null || element.email == '')
              && (element.existeConocimiento == null)
            )) {
            num_sindiligenciar = num_sindiligenciar + 1;
          } else if (!element.registroCompleto) {
            num_enproceso = num_enproceso + 1;
          } else if (element.registroCompleto) {
            num_completo = num_completo + 1;
          }
        }
      });
      if (num_sindiligenciar >= num_convocados) {
        retorno = 0;
      } else if (num_enproceso > 0 || (num_completo > 0 && num_completo < num_convocados)) {
        retorno = 1;
      } else if (num_completo >= num_convocados) {
        retorno = 2;
      }
    }

    return retorno;
  }
  estaIncompletodetalle(defensaJudicial: DefensaJudicial): number {
    let retorno:number=0;
    if(defensaJudicial.localizacionIdMunicipio!=null &&
      defensaJudicial.tipoAccionCodigo!=null &&
      defensaJudicial.jurisdiccionCodigo!=null &&
      defensaJudicial.pretensiones!="" && defensaJudicial.pretensiones!=null
       && defensaJudicial.esRequiereSupervisor!=null)
      {
        retorno= 2;
      }
      else{
        if(defensaJudicial.localizacionIdMunicipio==null &&
          defensaJudicial.tipoAccionCodigo==null &&
          defensaJudicial.jurisdiccionCodigo==null &&
          (defensaJudicial.pretensiones=="" || defensaJudicial.pretensiones==null)
           && defensaJudicial.esRequiereSupervisor==null)
          {
            retorno=0;
          }
          else
          {
            retorno=1;
          }

      }
    return retorno;
  }

  estaIncompletocontratos(pProceso:DefensaJudicial):number{

    let retorno:number=0;
    let num_contratos: number = 0;
    let listaContratos:any[]= [];

    if(pProceso != null){

      num_contratos = pProceso.cantContratos;// total de contratos


      this.defensaJudicial.defensaJudicialContratacionProyecto.forEach(element => {
        if (!listaContratos.includes(element.contratacionProyecto.contratacionId))
          listaContratos.push(element.contratacionProyecto.contratacionId);
      });
    }
      if(num_contratos > 0){
        if(listaContratos.length >= num_contratos){
          retorno = 2;
        }else if(listaContratos.length > 0 && listaContratos.length < num_contratos ){
          retorno = 1;
        }
      }else{
        retorno = 0;
      }

    return retorno;
  }

  ngOnInit(): void {
    this.textCabecera="Registrar nuevo proceso de defensa judicial";
    this.activatedRoute.params.subscribe( param => {
      this.controlJudicialId = param['id'];

      if(this.controlJudicialId)
      {

        this.editMode();

      }
    });
    this.commonServices.listaProcesosJudiciales().subscribe(
      response=>{
        this.tipoProcesoArray=response;
      }
    );

  }

}
