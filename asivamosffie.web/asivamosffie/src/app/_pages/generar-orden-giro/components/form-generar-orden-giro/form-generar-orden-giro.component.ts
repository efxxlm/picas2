import { ActivatedRoute } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { OrdenPagoService } from 'src/app/core/_services/ordenPago/orden-pago.service';

@Component({
  selector: 'app-form-generar-orden-giro',
  templateUrl: './form-generar-orden-giro.component.html',
  styleUrls: ['./form-generar-orden-giro.component.scss']
})
export class FormGenerarOrdenGiroComponent implements OnInit {

    constructor(
        private activatedRoute: ActivatedRoute,
        private ordenPagoSvc: OrdenPagoService )
    {
        this.ordenPagoSvc.getSolicitudPagoBySolicitudPagoId( this.activatedRoute.snapshot.params.id )
            .subscribe(
                response => {
                    console.log( response );
                }
            );
    }

    ngOnInit(): void {
    }

}
