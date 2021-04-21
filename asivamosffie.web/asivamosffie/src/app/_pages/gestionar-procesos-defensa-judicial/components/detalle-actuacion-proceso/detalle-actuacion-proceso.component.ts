import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { DefensaJudicial, DefensaJudicialService } from 'src/app/core/_services/defensaJudicial/defensa-judicial.service';

@Component({
  selector: 'app-detalle-actuacion-proceso',
  templateUrl: './detalle-actuacion-proceso.component.html',
  styleUrls: ['./detalle-actuacion-proceso.component.scss']
})
export class DetalleActuacionProcesoComponent implements OnInit {
  controlJudicialId: any;
  defensaJudicial: DefensaJudicial={};
  defensaJudicialId: number;

  constructor(private fb: FormBuilder, public dialog: MatDialog,
    public commonServices: CommonService,
    public judicialServices:DefensaJudicialService,
    private activatedRoute: ActivatedRoute, private router: Router) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe( param => {
      this.controlJudicialId = param['id'];    
      this.judicialServices.GetDefensaJudicialById(this.controlJudicialId).subscribe(respose=>{
        this.defensaJudicial=respose;
        this.defensaJudicialId = this.defensaJudicial.defensaJudicialId;
      });
    });
  }
}


