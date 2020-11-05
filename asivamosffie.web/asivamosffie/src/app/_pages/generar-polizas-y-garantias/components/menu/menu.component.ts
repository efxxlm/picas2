import { Component, OnInit } from '@angular/core';
import { PolizaGarantiaService } from 'src/app/core/_services/polizaGarantia/poliza-garantia.service';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.scss']
})
export class MenuComponent implements OnInit {

  verAyuda = false;
  public dataTable;
  constructor(private polizaService: PolizaGarantiaService) { }

  ngOnInit(): void {
    this.loadServicePrueba();
  }
  loadServicePrueba(){
    this.polizaService.GetListGrillaContratoGarantiaPoliza().subscribe(data=>{
      this.loadTableData(data);
    });
  }
  loadTableData(data){
    this.dataTable = data;
  }
}
