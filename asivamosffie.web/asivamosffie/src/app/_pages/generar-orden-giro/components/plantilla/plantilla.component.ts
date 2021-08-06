import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { OrdenPagoService } from 'src/app/core/_services/ordenPago/orden-pago.service';

@Component({
  selector: 'app-plantilla',
  templateUrl: './plantilla.component.html',
  styleUrls: ['./plantilla.component.scss']
})
export class PlantillaComponent implements OnInit {

    ordenGiro = undefined;

    constructor(
        private activatedRoute: ActivatedRoute,
        private ordenPagoSvc: OrdenPagoService )
    {
        this.getOrdenGiro()
    }

    ngOnInit(): void {
    }

    async getOrdenGiro() {
        const ordenGiro = await this.ordenPagoSvc.getSolicitudPagoBySolicitudPagoId( this.activatedRoute.snapshot.params.id ).toPromise()

        console.log( ordenGiro )
        this.ordenGiro = ordenGiro
    }

}
