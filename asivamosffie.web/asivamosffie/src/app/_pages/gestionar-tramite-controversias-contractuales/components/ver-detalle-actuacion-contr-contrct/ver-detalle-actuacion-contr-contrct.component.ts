import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ContractualControversyService } from 'src/app/core/_services/ContractualControversy/contractual-controversy.service';

@Component({
  selector: 'app-ver-detalle-actuacion-contr-contrct',
  templateUrl: './ver-detalle-actuacion-contr-contrct.component.html',
  styleUrls: ['./ver-detalle-actuacion-contr-contrct.component.scss']
})
export class VerDetalleActuacionContrContrctComponent implements OnInit {
  idActuacion: any;

  constructor(private activatedRoute: ActivatedRoute,private services: ContractualControversyService) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(param => {
      this.idActuacion = param.id;
    });
  }

}
