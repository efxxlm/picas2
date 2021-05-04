import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, UrlSegment } from '@angular/router';
import { FinancialBalanceService } from 'src/app/core/_services/financialBalance/financial-balance.service';

@Component({
  selector: 'app-validar-balance-gbftrec',
  templateUrl: './validar-balance-gbftrec.component.html',
  styleUrls: ['./validar-balance-gbftrec.component.scss']
})
export class ValidarBalanceGbftrecComponent implements OnInit {
  id: number;
  opcion1 = false;
  opcion2 = false;
  opcion3 = false;
  data : any;
  esRegistroNuevo: boolean;
  esVerDetalle: boolean;
  constructor(
    private route: ActivatedRoute,
    private financialBalanceService: FinancialBalanceService,
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe((params: Params) => {
      this.id = params.id;
    });
    this.route.snapshot.url.forEach( ( urlSegment: UrlSegment ) => {
      if ( urlSegment.path === 'validarBalance' ) {
          this.esVerDetalle = false;
          this.esRegistroNuevo = true;
          return;
      }
      if ( urlSegment.path === 'verDetalleEditarBalance' ) {
          this.esVerDetalle = false;
          this.esRegistroNuevo = false;
          return;
      }
      if ( urlSegment.path === 'verDetalleBalance' ) {
          this.esVerDetalle = true;
          return;
      }
    });
    if(this.id != null){
      this.financialBalanceService.getDataByProyectoId(this.id).subscribe(data => {
          if(data.length > 0){
            this.data = data[0];
          }
      });
    }
  }

}
