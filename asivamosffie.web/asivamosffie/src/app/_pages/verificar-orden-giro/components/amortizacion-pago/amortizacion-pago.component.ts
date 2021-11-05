import { Component, Input, OnInit } from '@angular/core';
import { CommonService } from 'src/app/core/_services/common/common.service';

@Component({
  selector: 'app-amortizacion-pago',
  templateUrl: './amortizacion-pago.component.html',
  styleUrls: ['./amortizacion-pago.component.scss']
})
export class AmortizacionPagoComponent implements OnInit {

    @Input() solicitudPagoFaseAmortizacion: any;
    @Input() contratacionProyectoId: number;
    @Input() vAmortizacionXproyecto: any;

    valorAmortizacion = 0;
    llaveMen: any;
    saldoAmortizar = 0;

    constructor(private commonSvc: CommonService)
    { }

    ngOnInit(): void {
        if ( this.solicitudPagoFaseAmortizacion != null ) {
            this.valorAmortizacion = this.solicitudPagoFaseAmortizacion.valorAmortizacion !== undefined ? this.solicitudPagoFaseAmortizacion.valorAmortizacion : null
            this.saldoAmortizar = this.vAmortizacionXproyecto?.valorPorAmortizar !== undefined ? this.vAmortizacionXproyecto?.valorPorAmortizar : null;
            this.commonSvc.getLlaveMenByContratacionProyectoId(this.contratacionProyectoId)
            .subscribe((data)=>{
              this.llaveMen = data[0]?.llaveMen;
            })
          }
    }

}
