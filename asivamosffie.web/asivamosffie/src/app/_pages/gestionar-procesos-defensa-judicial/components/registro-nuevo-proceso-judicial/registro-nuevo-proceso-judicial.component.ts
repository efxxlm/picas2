import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
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

  ngOnInit(): void {
    this.activatedRoute.params.subscribe( param => {
      this.controlJudicialId = param['id'];
      if(this.controlJudicialId)
      {
        this.judicialServices.GetDefensaJudicialById(this.controlJudicialId).subscribe(response=>{
          this.defensaJudicial=response;
          console.log(response);
          this.addressForm.get("legitimacionActiva").setValue(response.esLegitimacionActiva);
          this.addressForm.get("tipoProceso").setValue(response.tipoProcesoCodigo);

        });
      }
    });
    this.commonServices.listaProcesosJudiciales().subscribe(
      response=>{
        this.tipoProcesoArray=response;
      }
    );

  }

}
