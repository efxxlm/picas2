import { Component, Input, OnInit } from '@angular/core';
import { Dominio } from 'src/app/core/_services/common/common.service';
import { MatTableDataSource } from '@angular/material/table';
import { ReleaseBalanceService } from 'src/app/core/_services/releaseBalance/release-balance.service';
import { FinancialBalanceService } from 'src/app/core/_services/financialBalance/financial-balance.service';

@Component({
  selector: 'app-ver-liberacion-saldos',
  templateUrl: './ver-liberacion-saldos.component.html',
  styleUrls: ['./ver-liberacion-saldos.component.scss']
})
export class VerLiberacionSaldosComponent implements OnInit {

    @Input() proyectoId: number ;
    proyecto: any;
    balanceFinanciero: any;
    listaEstadoTraslado: Dominio[] = [];
    dataSource = new MatTableDataSource();
    urlDetalle: string;
    drps: any[] = [];

    constructor(
        private balanceSvc: FinancialBalanceService,
        private releaseBalanceService: ReleaseBalanceService)
    {
    }

    async ngOnInit() {
      const getDataByProyectoId = await this.balanceSvc.getDataByProyectoId(this.proyectoId).toPromise()
      if( getDataByProyectoId.length > 0 ){
          this.proyecto = getDataByProyectoId[0]
      }
      this.getDrpByProyectoId();
    }

    getDrpByProyectoId() {
      this.releaseBalanceService.getDrpByProyectoId(this.proyectoId).subscribe(data => {
          this.drps = data;
      });
    }

}
