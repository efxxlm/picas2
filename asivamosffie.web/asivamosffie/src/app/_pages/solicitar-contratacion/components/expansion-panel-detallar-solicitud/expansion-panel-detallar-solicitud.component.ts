import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';

@Component({
  selector: 'app-expansion-panel-detallar-solicitud',
  templateUrl: './expansion-panel-detallar-solicitud.component.html',
  styleUrls: ['./expansion-panel-detallar-solicitud.component.scss']
})
export class ExpansionPanelDetallarSolicitudComponent implements OnInit {

  constructor(
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe((params: Params) => {
      console.log(params);
      const id = params.id;
      console.log(id);
    });
  }

}
