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
          setTimeout(() => { resolve(); },1000)
      });           
    });

    
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
