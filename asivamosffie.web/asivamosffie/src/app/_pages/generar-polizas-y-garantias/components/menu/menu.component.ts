import { Component, OnInit } from '@angular/core';
import { PolizaGarantiaService } from 'src/app/core/_services/polizaGarantia/poliza-garantia.service';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.scss']
})
export class MenuComponent implements OnInit {
  estadoAcordeon: string;
  estadoAcordeon1: string;
  estadoAcordeon2: string;
  estadoAcordeon3: string;
  verAyuda = false;
  dataTable = [];
  constructor(private polizaService: PolizaGarantiaService) { }

  ngOnInit(): void {
    /*
    this.polizaService.GetListGrillaContratoGarantiaPoliza().subscribe(data=>{
      this.polizaService.loadDataItems.next(data);
    });
    */
  }
}
