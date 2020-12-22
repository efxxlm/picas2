import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
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
  soporte_class:number=0;
  ficha_class:number=0;

  constructor(private fb: FormBuilder, public dialog: MatDialog, 
    public commonServices: CommonService,
    public judicialServices:DefensaJudicialService,
    private activatedRoute: ActivatedRoute,) { }

    async editMode(){
    
    
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
          console.log(this.defensaJudicial); 
          this.contratos_class=this.estaIncompletocontratos(this.defensaJudicial);
          this.detalle_class=this.estaIncompletodetalle(this.defensaJudicial);
          this.convocados_class=this.estaIncompletoconvocados(this.defensaJudicial);
          this.soporte_class=this.defensaJudicial.urlSoporteProceso?2:1;
          this.ficha_class=this.estaIncompletoficha(this.defensaJudicial);
          setTimeout(() => { resolve(); },1000)
      });           
    });

    
  }
  estaIncompletoficha(defensaJudicial: DefensaJudicial): number {
    let retorno:number=0;
    if(defensaJudicial>0)
      {
        retorno= 2;
      }
      else{       
      retorno=1;
      }    
    return retorno;
  }
  estaIncompletoconvocados(defensaJudicial: DefensaJudicial): number {
    let retorno:number=0;
    if(defensaJudicial.demandadoConvocado.length>0)
      {
        retorno= 2;
      }
      else{       
      retorno=1;
      }    
    return retorno;
  }
  estaIncompletodetalle(defensaJudicial: DefensaJudicial): number {
    let retorno:number=0;
    if(defensaJudicial.localizacionIdMunicipio!=null && 
      defensaJudicial.tipoAccionCodigo!=null &&
      defensaJudicial.jurisdiccionCodigo!=null &&
      defensaJudicial.pretensiones!="" && defensaJudicial.pretensiones!=null &&
      defensaJudicial.cuantiaPerjuicios>0 && defensaJudicial.esRequiereSupervisor!=null)
      {
        retorno= 2;
      }
      else{       
      retorno=1;
      }    
    return retorno;
  }

  estaIncompletocontratos(pProceso:DefensaJudicial):number{        
    
    let retorno:number=0;
    if(pProceso.cantContratos>0 && pProceso.defensaJudicialContratacionProyecto.length>0)
      {
        retorno= 2;
      }
      else{       
      retorno=1;
      }    
    return retorno;
  }

  ngOnInit(): void {
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
