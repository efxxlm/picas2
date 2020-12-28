import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { DefensaJudicialService } from 'src/app/core/_services/defensaJudicial/defensa-judicial.service';

@Component({
  selector: 'app-actualizar-proceso-dj',
  templateUrl: './actualizar-proceso-dj.component.html',
  styleUrls: ['./actualizar-proceso-dj.component.scss']
})
export class ActualizarProcesoDjComponent implements OnInit {
  controlJudicialId: any;
  defensaJudicial: any;

  constructor(public dialog: MatDialog, 
    public commonServices: CommonService,
    public judicialServices:DefensaJudicialService,
    private activatedRoute: ActivatedRoute,) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe( param => {
      this.controlJudicialId = param['id'];
      this.judicialServices.GetDefensaJudicialById(this.controlJudicialId).subscribe(respose=>{
        this.defensaJudicial=respose;
      });
    });
  }

}
