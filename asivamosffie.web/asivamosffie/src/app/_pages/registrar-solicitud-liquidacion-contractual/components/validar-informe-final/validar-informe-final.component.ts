import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute, Params } from '@angular/router';
import { RegisterContractualLiquidationRequestService } from 'src/app/core/_services/registerContractualLiquidationRequest/register-contractual-liquidation-request.service';
import { InformeFinal } from 'src/app/_interfaces/informe-final';

@Component({
  selector: 'app-validar-informe-final',
  templateUrl: './validar-informe-final.component.html',
  styleUrls: ['./validar-informe-final.component.scss']
})
export class ValidarInformeFinalComponent implements OnInit {

  proyectoId: number;
  data: any;
  constructor(
    private route: ActivatedRoute,
    private registerContractualLiquidationRequestService: RegisterContractualLiquidationRequestService
  ) { 
    this.route.params.subscribe((params: Params) => {
      this.proyectoId = params.proyectoId;
    });
  }

  ngOnInit(): void {
    this.getInformeFinalByProyectoId(this.proyectoId);
  }

  getInformeFinalByProyectoId(proyectoId: number) {
    this.registerContractualLiquidationRequestService.getInformeFinalByProyectoId(proyectoId).subscribe(report => {
      if(report != null){
        this.data = report[0];
      }
    });
  }

}
